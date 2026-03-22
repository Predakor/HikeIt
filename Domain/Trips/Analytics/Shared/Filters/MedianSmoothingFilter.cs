using Application.Commons.Abstractions;
using Domain.AppSettings.Settings;
using Domain.Trips.Root.Builders.GpxDataBuilder;

namespace Domain.Trips.Analytics.Shared.Filters;

public sealed class MedianSmoothingFilter : IFilter<MutableGpxPoint>
{
    public sealed record Config(int WindowSize) : FilterConfigBase<Config>(default!);

    private readonly int _windowSize;
    public MedianSmoothingFilter(Config config)
    {
        _windowSize = config.WindowSize;
    }
    public IList<MutableGpxPoint> Apply(IList<MutableGpxPoint> values)
    {
        var sortedWindow = new List<double>(_windowSize);

        for (int i = 0; i < values.Count; i++)
        {
            double currentEle = values[i].Ele;
            int insertIdx = sortedWindow.BinarySearch(currentEle);
            if (insertIdx < 0)
            {
                insertIdx = ~insertIdx;
            }

            sortedWindow.Insert(insertIdx, currentEle);

            if (sortedWindow.Count > _windowSize)
            {
                var oldEle = values[i - _windowSize].Ele;
                int removeIdx = sortedWindow.BinarySearch(oldEle);
                if (removeIdx > 0)
                {
                    sortedWindow.RemoveAt(removeIdx);
                }
            }

            values[i].Ele = sortedWindow[sortedWindow.Count / 2];
        }
        return values;
    }
}