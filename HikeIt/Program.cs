using Api.DI;
using Infrastructure;
using Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

var isDevelopment = builder.Environment.IsDevelopment();

builder.InjectAppConfig();

builder.Services.AddInfrastructure(builder.Configuration, isDevelopment);

builder.Services.AddControllers();

builder.InjectSwagger();
builder.InjectIdentity();
builder.InjectServices();

var app = builder.Build();

await MigrationHelper.MigrateDatabaseAsync(app.Services);

Console.WriteLine($"Current Directory in api: {Directory.GetCurrentDirectory()}");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI(c => {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "HikeIT Api v1");
    });

    app.UseCors(policy => {
        policy
            .WithOrigins("http://localhost:54840")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .WithExposedHeaders("Location");
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapEndpoints();
app.MapControllers();

app.Run();
