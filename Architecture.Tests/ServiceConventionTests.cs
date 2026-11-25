using Api.Commons.Extentions;

namespace Architecture.Tests;

public class ServiceConventionTests
{
    [Fact]
    public void Every_service_interface_has_an_implementation()
    {
        var withoutImplementation = ProductionTypes
            .ServiceInterfaces.Where(@interface =>
                !ProductionTypes.ServiceImplementations.Any(@interface.IsAssignableFrom)
            )
            .ToArray();

        Assert.True(
            withoutImplementation.Length == 0,
            $"Service interfaces without an implementation:{Environment.NewLine}{withoutImplementation.Describe()}"
        );
    }

    // The scan registers every implementation, so a second one silently replaces the first (last registration wins).
    [Fact]
    public void Every_service_interface_has_exactly_one_implementation()
    {
        var ambiguous = ProductionTypes
            .LeafServiceInterfaces.Where(@interface =>
                ProductionTypes.ServiceImplementations.Count(@interface.IsAssignableFrom) > 1
            )
            .Select(@interface =>
                $"{@interface.FullName}: {string.Join(", ", ProductionTypes.ServiceImplementations.Where(@interface.IsAssignableFrom).Select(type => type.Name))}"
            )
            .ToArray();

        Assert.True(
            ambiguous.Length == 0,
            $"Service interfaces with several implementations:{Environment.NewLine}{string.Join(Environment.NewLine, ambiguous)}"
        );
    }

    // Singletons are named by role (cache, queue, publisher); scoped services are use-case services.
    [Fact]
    public void Every_scoped_service_implementation_ends_with_Service()
    {
        var misnamed = ProductionTypes
            .ServiceImplementations.Where(type => typeof(IService).IsAssignableFrom(type))
            .Where(type => !type.Name.EndsWith("Service", StringComparison.Ordinal))
            .ToArray();

        Assert.True(
            misnamed.Length == 0,
            $"Scoped service implementations not named *Service:{Environment.NewLine}{misnamed.Describe()}"
        );
    }

    [Fact]
    public void Every_decorator_implements_the_service_it_decorates()
    {
        var wrong = ProductionTypes
            .Decorators.Where(decorator =>
                !decorator
                    .GetDecoratedService()
                    .IsAssignableFrom(decorator)
            )
            .ToArray();

        Assert.True(
            wrong.Length == 0,
            $"Decorators that do not implement their service:{Environment.NewLine}{wrong.Describe()}"
        );
    }

    [Fact]
    public void Every_decorator_takes_the_decorated_service_in_its_constructor()
    {
        var wrong = ProductionTypes
            .Decorators.Where(decorator =>
                !decorator
                    .GetConstructors()
                    .Any(constructor =>
                        constructor
                            .GetParameters()
                            .Any(parameter => parameter.ParameterType == decorator.GetDecoratedService())
                    )
            )
            .ToArray();

        Assert.True(
            wrong.Length == 0,
            $"Decorators without the decorated service as a constructor parameter:{Environment.NewLine}{wrong.Describe()}"
        );
    }

    [Fact]
    public void Decorated_service_has_an_implementation_to_wrap()
    {
        var withoutTarget = ProductionTypes
            .Decorators.Select(decorator => decorator.GetDecoratedService())
            .Distinct()
            .Where(service =>
                !ProductionTypes.Concrete.Any(type => !type.IsDecorator() && service.IsAssignableFrom(type))
            )
            .ToArray();

        Assert.True(
            withoutTarget.Length == 0,
            $"Decorated services without an implementation:{Environment.NewLine}{withoutTarget.Describe()}"
        );
    }

    // A decorator that is not marked is scanned as a plain implementation and then wraps itself, which hangs resolution.
    [Fact]
    public void Unmarked_classes_wrapping_a_service_they_implement_are_not_hidden_decorators()
    {
        var hidden = ProductionTypes
            .ServiceImplementations.Where(type =>
                type
                    .GetInterfaces()
                    .Where(@interface => ProductionTypes.ServiceInterfaces.Contains(@interface))
                    .Any(service =>
                        type
                            .GetConstructors()
                            .Any(constructor =>
                                constructor
                                    .GetParameters()
                                    .Any(parameter => parameter.ParameterType == service)
                            )
                    )
            )
            .ToArray();

        Assert.True(
            hidden.Length == 0,
            $"Classes take their own service interface in the constructor but carry no decorator marker:{Environment.NewLine}{hidden.Describe()}"
        );
    }
}
