namespace Application.Trips.Analytics.Pipeline.Contracts;

public abstract record StageData
{
    public sealed record Cleanup(int PointCountIn, int PointCountOut) : StageData;

    public sealed record MatchedPeak(int PeakId, string Name, double DistanceM, double Elevation);

    public sealed record PeakMatching(int CandidateCount, IReadOnlyList<MatchedPeak> Matched) : StageData;
}
