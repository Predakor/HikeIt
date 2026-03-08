using Api;
using Api.DI;
using Application;
using Infrastructure;
using Infrastructure.Commons.Databases.Utils;
using Microsoft.AspNetCore.HttpOverrides;

Console.WriteLine("test");
var builder = WebApplication.CreateBuilder(args);

var isDevelopment = builder.Environment.IsDevelopment();

builder.InjectAppConfig();

builder.Services
    .AddApi(builder.Configuration)
    .AddInfrastructure(builder.Configuration, isDevelopment)
    .AddAplication()
    .AddControllers();

builder.AddLogger();

var app = builder.Build();

var basePath = app.Configuration.GetSection("Cors:BasePath").Get<string>() ?? "/";
Console.WriteLine($"basePath:{basePath}");
app.UsePathBase(basePath);

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

await DbHelpers.TryMigrationAndSeeding(app.Services, app.Configuration);

Console.WriteLine($"Current Directory in api: {Directory.GetCurrentDirectory()}");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "HikeIT Api v1");
    });
}

var allowedOrigins =
    app.Configuration.GetSection("Cors:AllowedOrigins").Get<string>().Split(",").ToArray()
    ?? throw new Exception("allowed origins are undefined");
Console.WriteLine($"Allowed Origins: {string.Join(",", allowedOrigins)}");

app.UseHttpsRedirection();

app.UseCors(policy =>
{
    policy
        .WithOrigins(allowedOrigins)
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials()
        .WithExposedHeaders("Location");
});

app.UseAuthentication();
app.UseAuthorization();

app.MapEndpoints();
app.MapControllers();

await app.RunAsync();
