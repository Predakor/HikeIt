using Domain.Peaks;

namespace Domain.Trips.Analytics.Shared;

// The state the peak filters refine. Peaks is how the lookup filter reaches known peaks.
public sealed class PeakChain(IList<PeakCandidate> candidates, IPeakSource peaks)
{
    private int? _candidatesBeforeLookup;

    public IList<PeakCandidate> Candidates { get; set; } = candidates;
    public IPeakSource Peaks { get; } = peaks;

    // How many candidates reached the lookup; all of them when no lookup ran.
    public int CandidatesBeforeLookup => _candidatesBeforeLookup ?? Candidates.Count;

    public void MarkLookupStarted()
    {
        _candidatesBeforeLookup ??= Candidates.Count;
    }

    public IEnumerable<PeakCandidate> Matched => Candidates.Where(candidate => candidate.Peak is not null);
}
