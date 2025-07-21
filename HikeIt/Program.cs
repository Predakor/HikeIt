using Api.Configuration.Cors.Factories;
using Api.Configuration.Cors.Models;
using Api.DI;
using Infrastructure;
using Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

builder.InjectAppConfig();

var dbString =
    builder.Configuration.GetConnectionString("TripDbCS")
    ?? throw new Exception("DbConnectionString is empty or null");

builder.Services.AddDatabase(dbString);
builder.Services.AddControllers();

builder.InjectSwagger();
builder.InjectIdentity();
builder.InjectServices();

var corsConfig = ConfigureCors(builder);

var app = builder.Build();

await MigrationHelper.MigrateDatabaseAsync(app.Services);

Console.WriteLine($"Current Directory in api: {Directory.GetCurrentDirectory()}");

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

app.MapEndpoints();
app.MapControllers();

app.Run();

static CorsConfig ConfigureCors(WebApplicationBuilder builder) {
    var corsConfig = CorsConfigFactory.Create(builder.Environment, builder.Configuration);
    CorsPolicyFactory.Create(corsConfig).ApplyCorsPolicy(builder.Services);
    return corsConfig;
}
