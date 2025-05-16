using HikeIt.Api.Configuration.Cors.Factories;
using HikeIt.Api.Data;
using HikeIt.Api.Endpoints;
using HikeIt.Api.Entities;
using HikeIt.Api.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Use connnection string from appsettings.json
builder.Services.AddDbContext<TripDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("TripDbCS"))
);

InjectRepositories(builder);

var corsConfig = CorsConfigFactory.Create(builder.Environment, builder.Configuration);
CorsPolicyFactory.Create(corsConfig).ApplyCorsPolicy(builder.Services);

var app = builder.Build();

using (var scope = app.Services.CreateScope()) {
    var services = scope.ServiceProvider;
    try {
        var context = services.GetRequiredService<TripDbContext>();
        await context.Database.MigrateAsync();
    }
    catch (Exception ex) {
        Console.WriteLine($"error {ex}");
    }
}

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
}

static void InjectRepositories(WebApplicationBuilder builder) {
    builder.Services.AddScoped<IRepository<Trip>, SqlRepository<Trip>>();
    builder.Services.AddScoped<IRepository<Peak>, SqlRepository<Peak>>();
    builder.Services.AddScoped<IRepository<User>, SqlRepository<User>>();
}