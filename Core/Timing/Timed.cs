using System.Diagnostics;

namespace Core.Timing;

public static class Timed
{
    public static async Task<(TimeSpan Elapsed, TResult Result)> RunAsync<TResult>(Func<Task<TResult>> work)
    {
        var start = Stopwatch.GetTimestamp();
        var result = await work();

        return (Stopwatch.GetElapsedTime(start), result);
    }

    public static async Task<(TimeSpan Elapsed, TResult Result)> RunAsync<TResult>(Func<ValueTask<TResult>> work)
    {
        var start = Stopwatch.GetTimestamp();
        var result = await work();

        return (Stopwatch.GetElapsedTime(start), result);
    }
}