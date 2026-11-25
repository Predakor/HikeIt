using System.Reflection;

namespace Architecture.Tests;

public class LayerDependencyTests
{
    private static readonly Dictionary<string, Assembly> layers = new()
    {
        ["Core"] = ProductionTypes.Core,
        ["Domain"] = ProductionTypes.Domain,
        ["Application"] = ProductionTypes.Application,
        ["Infrastructure"] = ProductionTypes.Infrastructure,
        ["Api"] = ProductionTypes.Api,
    };

    // Layer -> layers it must not reference. Api -> Application -> Domain, Infrastructure implements, Core everywhere.
    private static readonly Dictionary<string, string[]> forbidden = new()
    {
        ["Core"] = ["Domain", "Application", "Infrastructure", "Api"],
        ["Domain"] = ["Application", "Infrastructure", "Api"],
        ["Application"] = ["Infrastructure", "Api"],
        ["Infrastructure"] = ["Api"],
        ["Api"] = [],
    };

    public static TheoryData<string> LayerNames => [.. forbidden.Keys];

    [Theory]
    [MemberData(nameof(LayerNames))]
    public void Layer_does_not_reference_a_layer_above_it(string layer)
    {
        var referenced = layers[layer]
            .GetReferencedAssemblies()
            .Select(name => name.Name)
            .ToHashSet();

        var violations = forbidden[layer]
            .Where(referenced.Contains)
            .ToArray();

        Assert.True(violations.Length == 0, $"{layer} references {string.Join(", ", violations)}");
    }

    [Fact]
    public void Api_does_not_reach_into_Domain_internals_through_Infrastructure()
    {
        var infrastructureUsers = layers
            .Where(layer => layer.Key is not "Api" and not "Infrastructure")
            .Where(layer =>
                layer
                    .Value.GetReferencedAssemblies()
                    .Any(name => name.Name == "Infrastructure")
            )
            .Select(layer => layer.Key)
            .ToArray();

        Assert.Empty(infrastructureUsers);
    }
}
