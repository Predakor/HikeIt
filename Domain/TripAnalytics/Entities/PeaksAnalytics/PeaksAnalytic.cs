using Domain.Interfaces;
using Domain.ReachedPeaks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.TripAnalytics.Entities.PeaksAnalytics;

public class PeaksAnalytic : IEntity<Guid> {
    public Guid Id { get; init; }

    //TODO actually implement
    public PeakSummary? Summary { get; set; }

    [NotMapped]
    public ICollection<ReachedPeak> ReachedPeaks { get; set; } = [];

    public static PeaksAnalytic Create(Guid id, List<ReachedPeak> peaks) {
        return new() { Id = id, ReachedPeaks = peaks };
    }
}

//owned type will it be
public class PeakSummary {

    //number of  total reached peaks
    //highest peak
    //number of total unique peaks
    //number of peaks visited first time

}
