using Api.DI;
using Infrastructure.Data;
using Infrastructure.DI;

var builder = WebApplication.CreateBuilder(args);

builder.InjectAppConfig();

var dbString =
    builder.Configuration.GetConnectionString("TripDbCS")
    ?? throw new Exception("DbConnectionString is empty or null");

builder.Services.AddDatabase(dbString, builder.Environment.IsDevelopment());
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
