using Api.Configuration.Cors.Factories;
using Api.Configuration.Cors.Models;
using Api.Endpoints;
using Application.Mappers.Implementations;
using Application.Services.Peaks;
using Application.Services.Region;
using Application.Services.Trip;
using Application.Services.Users;
using Domain.Peaks;
using Domain.Regions;
using Domain.Trips;
using Infrastructure;
using Infrastructure.Data;
using Infrastructure.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDatabase(builder.Configuration.GetConnectionString("TripDbCS"));

InjectRepositories(builder);
InjectMappers(builder);
InjectServices(builder);

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
    app.MapTripsEndpoint();
    app.MapPeaksEndpoint();
    app.MapUserEndpoints();
    app.MapRegionsEndpoints();
}

static void InjectRepositories(WebApplicationBuilder builder) {
    builder.Services.AddScoped<IPeakRepository, PeakRepository>();
    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddScoped<ITripRepository, TripRepository>();
    builder.Services.AddScoped<IRegionRepository, RegionRepository>();
}

static void InjectServices(WebApplicationBuilder builder) {
    builder.Services.AddScoped<PeakService>();
    builder.Services.AddScoped<UserService>();
    builder.Services.AddScoped<TripService>();
    builder.Services.AddScoped<RegionService>();
}
static void InjectMappers(WebApplicationBuilder builder) {
    builder.Services.AddScoped<PeakMapper>();
    builder.Services.AddScoped<TripMapper>();
    builder.Services.AddScoped<RegionMapper>();
}

static CorsConfig ConfigureCors(WebApplicationBuilder builder) {
    var corsConfig = CorsConfigFactory.Create(builder.Environment, builder.Configuration);
    CorsPolicyFactory.Create(corsConfig).ApplyCorsPolicy(builder.Services);
    return corsConfig;
}
