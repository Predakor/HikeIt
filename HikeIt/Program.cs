using Api.Configuration.Cors.Factories;
using Api.Configuration.Cors.Models;
using Api.Endpoints;
using Application.Mappers.Implementations;
using Application.Services.Files;
using Domain.TripAnalytics.Interfaces;
using Infrastructure;
using Infrastructure.Data;
using Infrastructure.Parsers;
using Infrastructure.Storage;
using Infrastructure.UnitOfWorks;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDatabase(builder.Configuration.GetConnectionString("TripDbCS"));

InjectDependencies(builder);

var corsConfig = ConfigureCors(builder);

var app = builder.Build();

await MigrationHelper.MigrateDatabaseAsync(app.Services);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(corsConfig.Name);

MapEndpoints(app);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

static void MapEndpoints(WebApplication app) {
    //app.MapTripsEndpoints();
    app.MapPeaksEndpoints();
    app.MapUserEndpoints();
    app.MapRegionsEndpoints();
    app.MapFilesEndpoints();
}

static void InjectDependencies(WebApplicationBuilder builder) {
    var assemblies = new[] { "Application", "Infrastructure", "Domain", "Api" }
        .Select(Assembly.Load)
        .ToArray();

    InjectMappers(builder);
    InjectRepositories(builder, assemblies);
    InjectStorages(builder);
    InjectServices(builder, assemblies);
    InjectParsers(builder);
    InjectUnitOfWorks(builder);
}

static void InjectParsers(WebApplicationBuilder builder) {
    builder.Services.AddScoped<IGpxParser, GpxParser>();
}

static void InjectStorages(WebApplicationBuilder builder) {
    builder.Services.AddScoped<IFileStorage, FileStorage>();
}

static void InjectRepositories(WebApplicationBuilder builder, Assembly[] targetAssemblies) {
    builder.Services.Scan(scan =>
        scan.FromAssemblies(targetAssemblies)
            .AddClasses(classes =>
                classes.Where(type =>
                    type.Name.EndsWith("Repository") && type.IsClass && !type.IsAbstract
                )
            )
            .AsImplementedInterfaces()
            .WithScopedLifetime()
    );
}

static void InjectServices(WebApplicationBuilder builder, Assembly[] targetAssemblies) {
    builder.Services.Scan(scan =>
        scan.FromAssemblies(targetAssemblies)
            .AddClasses(classes =>
                classes.Where(t => t.Name.EndsWith("Service") && t.IsClass && !t.IsAbstract)
            )
            .AsImplementedInterfaces()
            .WithScopedLifetime()
    );
}
static void InjectMappers(WebApplicationBuilder builder) {
    builder.Services.AddScoped<PeakMapper>();
    builder.Services.AddScoped<RegionMapper>();
}

static void InjectUnitOfWorks(WebApplicationBuilder builder) {
    builder.Services.AddScoped<ITripAnalyticUnitOfWork, TripAnalyticsUnitOfWork>();
}

static CorsConfig ConfigureCors(WebApplicationBuilder builder) {
    var corsConfig = CorsConfigFactory.Create(builder.Environment, builder.Configuration);
    CorsPolicyFactory.Create(corsConfig).ApplyCorsPolicy(builder.Services);
    return corsConfig;
}
