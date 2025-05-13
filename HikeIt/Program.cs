using HikeIt.Api.Configuration;
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

builder.Services.AddScoped<IRepository<Trip>, SqlRepository<Trip>>();
builder.Services.AddScoped<IRepository<Peak>, SqlRepository<Peak>>();

string prodCorsPolicy = "AllowLocalhost";
string devCorsPolicy = "DevCors";

CorsConfiguration cors = new(prodCorsPolicy, devCorsPolicy);
cors.Configure(builder, builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>());

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

if (app.Environment.IsProduction()) {
    app.UseCors(prodCorsPolicy);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors(devCorsPolicy);
}

app.MapTripsEndpoint();
app.MapPeaksEndpoint();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
