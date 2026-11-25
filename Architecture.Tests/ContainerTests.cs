using Microsoft.Extensions.DependencyInjection;

namespace Architecture.Tests;

public class ContainerTests(ContainerFixture container) : IClassFixture<ContainerFixture>
{
    private static readonly TimeSpan resolveTimeout = TimeSpan.FromSeconds(10);

    public static TheoryData<Type> ServiceInterfaces => [.. ContainerHost.RegisteredServices];

    [Fact]
    public void Container_builds_with_every_dependency_satisfied_and_no_captive_scopes()
    {
        Assert.NotNull(container.Services);
    }

    // Resolution runs on its own task: a decorator wrapping itself recurses forever instead of throwing.
    [Theory]
    [MemberData(nameof(ServiceInterfaces))]
    public void Service_interface_resolves_without_hanging(Type service)
    {
        var resolution = Task.Run(() =>
        {
            using var scope = container.Services.CreateScope();
            return scope.ServiceProvider.GetRequiredService(service);
        });

        Assert.True(
            resolution.Wait(resolveTimeout),
            $"Resolving {service.Name} did not finish within {resolveTimeout.TotalSeconds}s"
        );
        Assert.NotNull(resolution.Result);
    }
}
