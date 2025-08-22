using Api;
using Api.DI;
using Application;
using Infrastructure;
using Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

var isDevelopment = builder.Environment.IsDevelopment();

builder.InjectAppConfig();

builder.Services
    .AddApi()
    .AddInfrastructure(builder.Configuration, isDevelopment)
    .AddAplication()
    .AddControllers();

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

var allowedOrigins =
    app.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()
    ?? throw new Exception("allowed origins are undefined");

app.UseCors(policy => {
    policy
        .WithOrigins(allowedOrigins)
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials()
        .WithExposedHeaders("Location");
});

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapEndpoints();
app.MapControllers();

app.Run();
