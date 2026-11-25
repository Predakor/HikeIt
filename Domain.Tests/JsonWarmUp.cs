using System.Runtime.CompilerServices;
using Core.Serialization;
using Domain.Common.Abstractions;

namespace Domain.Tests;

internal static class JsonWarmUp
{
    // The first polymorphic deserialize costs ~50 ms (converter and reflection setup); pay it at assembly load so no test carries it.
    [ModuleInitializer]
    internal static void Run()
    {
        """{"filterType":"EmaSmoothingFilter","Alpha":0.5}""".FromJson<IPointFilterConfig>();
    }
}
