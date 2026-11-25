using Api;
using Api.DI;
using Application;
using Application.Commons.Options;
using Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Architecture.Tests;

internal static class ContainerHost
{
    // Configured like Program.cs does, from the web builder, so ASP.NET's own services are present; Development validates scopes and the graph.
    private static readonly Lazy<WebApplicationBuilder> builder = new(Configure);

    // Every project interface in the container, hand-registered ones included, so nothing escapes the resolution check.
    public static IEnumerable<Type> RegisteredServices { get; } =
        builder
            .Value.Services.Where(descriptor => !descriptor.IsKeyedService)
            .Select(descriptor => descriptor.ServiceType)
            .Where(type => type.IsInterface && !type.ContainsGenericParameters)
            .Where(type => ProductionTypes.All.Contains(type.Assembly))
            .Distinct()
            .ToArray();

    // Resolution runs on its own task: a decorator wrapping itself recurses forever instead of throwing.
    public static bool TryResolve(IServiceProvider services, Type service, TimeSpan timeout)
    {
        var resolution = Task.Run(() =>
        {
            using var scope = services.CreateScope();
            return scope.ServiceProvider.GetRequiredService(service);
        });

        try
        {
            return resolution.Wait(timeout);
        }
        catch (AggregateException)
        {
            return false;
        }
    }

    public static WebApplication Build()
    {
        return builder.Value.Build();
    }

    private static WebApplicationBuilder Configure()
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

        return builder;
    }
}

// Built once before the tests run, so no single test carries the cost.
public sealed class ContainerFixture : IAsyncLifetime
{
    private static readonly TimeSpan warmUpTimeout = TimeSpan.FromSeconds(10);

    private readonly WebApplication app = ContainerHost.Build();

    public IServiceProvider Services => app.Services;

    // The first resolution in the process costs ~600 ms (type loading, JIT, Identity/EF/Azure setup), whatever the service;
    // pay it here so no single test looks slow. Failures and hangs are reported by the per-service tests.
    public Task InitializeAsync()
    {
        foreach (var service in ContainerHost.RegisteredServices)
        {
            ContainerHost.TryResolve(app.Services, service, warmUpTimeout);
        }

        return Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        await app.DisposeAsync();
    }
}
