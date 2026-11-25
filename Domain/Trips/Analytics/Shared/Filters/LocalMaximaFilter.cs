using Domain.Common.Abstractions;

namespace Domain.Trips.Analytics.Shared.Filters;

/// <summary>Keeps the points the track rose into and falls away from. Needs the full track, so it must run first.</summary>
public sealed class LocalMaximaFilter(LocalMaximaFilter.Config config) : SyncFilter<PeakChain>
{
    public sealed record Config(double MinClimbM = 0) : IPeakFilterConfig
    {
        public IFilter<PeakChain> CreateFilter() => new LocalMaximaFilter(this);
    }

    public override PeakChain Apply(PeakChain chain)
    {
        chain.Candidates = FindMaxima(chain.Candidates);

        return chain;
    }

    private List<PeakCandidate> FindMaxima(IList<PeakCandidate> points)
    {
        List<PeakCandidate> maxima = [];
        bool hasRisenSinceLastPeak = false;
        double lowestSinceLastPeak =
            points.Count > 0
                ? points[0].Ele
                : 0;

        for (int i = 1; i < points.Count - 1; i++)
        {
            var current = points[i];

            bool risesIntoPoint = current.Ele >= points[i - 1].Ele;
            if (risesIntoPoint)
            {
                hasRisenSinceLastPeak = true;
            }

            bool isPeakShape = hasRisenSinceLastPeak && points[i + 1].Ele < current.Ele;
            if (isPeakShape)
            {
                hasRisenSinceLastPeak = false;

                bool climbedEnough = current.Ele - lowestSinceLastPeak >= config.MinClimbM;
                if (climbedEnough)
                {
                    maxima.Add(current);
                    lowestSinceLastPeak = current.Ele;
                }
            }

            lowestSinceLastPeak = Math.Min(lowestSinceLastPeak, current.Ele);
        }

        return maxima;
    }
}
