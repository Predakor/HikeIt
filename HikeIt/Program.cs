using Api.Configuration.Cors.Factories;
using Api.Configuration.Cors.Models;
using Api.DI;
using Api.Endpoints;
using Infrastructure;
using Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

builder
    .Configuration.SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", true)
    .AddEnvironmentVariables();



if (builder.Environment.IsDevelopment()) {
    builder.Configuration.AddUserSecrets<Program>();
}

// Add services to the container.
builder.Services.AddControllers();

var dbString = builder.Configuration.GetConnectionString("TripDbCS");

builder.Services.AddDatabase(dbString, builder.Environment.IsProduction());

builder.InjectSwagger();
builder.InjectIdentity();
builder.InjectServices();

var corsConfig = ConfigureCors(builder);

var app = builder.Build();

await MigrationHelper.MigrateDatabaseAsync(app.Services);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI(c => {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "HikeIT Api v1");
    });
}

app.UseCors(corsConfig.Name);

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

MapEndpoints(app);
app.MapControllers();

app.Run();

static void MapEndpoints(WebApplication app) {
    app.MapPeaksEndpoints();
    app.MapRegionsEndpoints();
    app.MapFilesEndpoints().RequireAuthorization();
}

static CorsConfig ConfigureCors(WebApplicationBuilder builder) {
    var corsConfig = CorsConfigFactory.Create(builder.Environment, builder.Configuration);
    CorsPolicyFactory.Create(corsConfig).ApplyCorsPolicy(builder.Services);
    return corsConfig;
}
