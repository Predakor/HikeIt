using Api;
using Api.DI;
using Application;
using Application.Commons.Options;
using Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Architecture.Tests;

public class ContainerTests
{
    private static readonly TimeSpan resolveTimeout = TimeSpan.FromSeconds(10);

    // Built like Program.cs does, from the web builder, so ASP.NET's own services are present; Development validates scopes and the graph.
    private static readonly Lazy<WebApplication> app = new(Build);

    // Every project interface in the container, hand-registered ones included, so nothing escapes the resolution check.
    private static readonly Lazy<Type[]> registeredServices = new(() =>
    {
        _ = app.Value;
        return [.. registeredTypes];
    });

    private static Type[] registeredTypes = [];

    private static WebApplication Build()
    {
        var builder = WebApplication.CreateBuilder(new WebApplicationOptions { EnvironmentName = "Development" });

        builder.Configuration.AddInMemoryCollection(
            new Dictionary<string, string?>
            {
                ["ConnectionStrings:TripDbCS"] = "Host=localhost;Database=architecture-tests;Username=x;Password=x",
                ["Storage:Account"] = "architecturetests",
                ["Storage:Key"] = Convert.ToBase64String(new byte[32]),
            }
        );

        // Bound here instead of InjectAppConfig, which would also load the developer's user secrets.
        builder.Services.Configure<StorageOptions>(builder.Configuration.GetSection(StorageOptions.Path));
        builder.Services.Configure<SeedingOptions>(builder.Configuration.GetSection(SeedingOptions.Seeding));

        builder
            .Services.AddApi(builder.Configuration)
            .AddInfrastructure(builder.Configuration, isDevelopment: true)
            .AddAplication()
            .DecorateMarkedServices()
            .AddControllers();

        registeredTypes =
        [
            .. builder
                .Services.Where(descriptor => !descriptor.IsKeyedService)
                .Select(descriptor => descriptor.ServiceType)
                .Where(type => type.IsInterface && !type.ContainsGenericParameters)
                .Where(type => ProductionTypes.All.Contains(type.Assembly))
                .Distinct(),
        ];

        return builder.Build();
    }

    public static TheoryData<Type> ServiceInterfaces => [.. registeredServices.Value];

    [Fact]
    public void Container_builds_with_every_dependency_satisfied_and_no_captive_scopes()
    {
        Assert.NotNull(app.Value.Services);
    }

    // Resolution runs on its own task: a decorator wrapping itself recurses forever instead of throwing.
    [Theory]
    [MemberData(nameof(ServiceInterfaces))]
    public void Service_interface_resolves_without_hanging(Type service)
    {
        var resolution = Task.Run(() =>
        {
            using var scope = app.Value.Services.CreateScope();
            return scope.ServiceProvider.GetRequiredService(service);
        });

        Assert.True(
            resolution.Wait(resolveTimeout),
            $"Resolving {service.Name} did not finish within {resolveTimeout.TotalSeconds}s"
        );
        Assert.NotNull(resolution.Result);
    }
}
