using Api;
using Api.Configuration.Cors.Factories;
using Api.Configuration.Cors.Models;
using Api.DI;
using Api.Endpoints;
using Infrastructure;
using Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddDatabase(builder.Configuration.GetConnectionString("TripDbCS"));

builder.InjectSwagger().InjectIdentity().InjectServices();

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

MapEndpoints(app);

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

static void MapEndpoints(WebApplication app) {
    app.MapPeaksEndpoints();
    app.MapRegionsEndpoints();
    app.MapUserEndpoints().RequireAuthorization();
    app.MapFilesEndpoints().RequireAuthorization();
}

static CorsConfig ConfigureCors(WebApplicationBuilder builder) {
    var corsConfig = CorsConfigFactory.Create(builder.Environment, builder.Configuration);
    CorsPolicyFactory.Create(corsConfig).ApplyCorsPolicy(builder.Services);
    return corsConfig;
}
