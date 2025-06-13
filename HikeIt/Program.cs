using Api.Configuration.Cors.Factories;
using Api.Configuration.Cors.Models;
using Api.Endpoints;
using Application.Mappers.Implementations;
using Application.Services.Files;
using Application.Services.Peaks;
using Application.Services.Region;
using Application.Services.Trips;
using Application.Services.Users;
using Application.TripAnalytics;
using Application.TripAnalytics.Interfaces;
using Application.TripAnalytics.Services;
using Domain.Entiites.Regions;
using Domain.Entiites.Users;
using Domain.ReachedPeaks;
using Domain.TripAnalytics.Interfaces;
using Domain.TripAnalytics.Repositories;
using Domain.TripAnalytics.Services;
using Domain.Trips;
using Domain.Trips.Entities.GpxFiles;
using Infrastructure;
using Infrastructure.Data;
using Infrastructure.Parsers;
using Infrastructure.Repository;
using Infrastructure.Storage;
using Infrastructure.UnitOfWorks;

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
    InjectMappers(builder);
    InjectRepositories(builder);
    InjectStorages(builder);
    InjectServices(builder);
    InjectParsers(builder);
    InjectUnitOfWorks(builder);
}


static void InjectParsers(WebApplicationBuilder builder) {
    builder.Services.AddScoped<IGpxParser, GpxParser>();
}

static void InjectStorages(WebApplicationBuilder builder) {
    builder.Services.AddScoped<IFileStorage, FileStorage>();
}

static void InjectRepositories(WebApplicationBuilder builder) {
    builder.Services.AddScoped<IPeakRepository, PeakRepository>();
    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddScoped<ITripRepository, TripRepository>();
    builder.Services.AddScoped<IRegionRepository, RegionRepository>();
    builder.Services.AddScoped<IGpxFileRepository, GpxFileRepository>();
    builder.Services.AddScoped<IReachedPeakRepository, ReachedPeakRepository>();
    builder.Services.AddScoped<IPeakAnalyticRepository, PeakAnalyticRepository>();
    builder.Services.AddScoped<ITripAnalyticRepository, TripAnalyticRepository>();
    builder.Services.AddScoped<IElevationProfileRepository, ElevationProfileRepository>();
}

static void InjectServices(WebApplicationBuilder builder) {
    builder.Services.AddScoped<IPeakService, PeakService>();
    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddScoped<ITripService, TripService>();
    builder.Services.AddScoped<IRegionService, RegionService>();
    builder.Services.AddScoped<IGpxFileService, GpxFileService>();
    builder.Services.AddScoped<ITripAnalyticService, TripAnalyticService>();
    builder.Services.AddScoped<IElevationProfileService, ElevationProfileService>();
    builder.Services.AddScoped<ITripDomainAnalyticService, TripDomainAnalyticsService>();

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
