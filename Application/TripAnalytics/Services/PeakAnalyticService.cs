using Application.TripAnalytics.Interfaces;
using Domain.Common;
using Domain.Common.Result;
using Domain.ReachedPeaks;
using Domain.TripAnalytics.Entities.PeaksAnalytics;

namespace Application.TripAnalytics.Services;

public class PeakAnalyticService(IPeakAnalyticRepository repository) : IPeakAnalyticService {
    readonly IPeakAnalyticRepository _repository = repository;

    public async Task<Result<PeaksAnalytic>> Create(IEnumerable<ReachedPeak> peaks) {
        var analytics = PeaksAnalytic.Create([.. peaks]);
        return await _repository.AddAsync(analytics);
    }

    public async Task<Result<PeaksAnalytic>> GeneratePeakAnalytic(IEnumerable<ReachedPeak> reachedPeaks) {

        if (!reachedPeaks.Any()) {
            return Errors.EmptyCollection("Reached Peaks");
        }

        var analytics = PeaksAnalytic.Create([.. reachedPeaks]);

        if (analytics == null) {
            return Errors.Unknown("Failed To create Peak Analytics");
        }

        return await _repository.AddAsync(analytics);

        //Create reached peaks
        //CreateReachedPeaksCommand
        //    .Create(new(reachedPeaks, trip.Id, userId))
        //    .Execute()
        //    .Match(
        //        async peaks => {
        //            var query = await unitOfWork.ReachedPeaks.AddRangeAsync(peaks);
        //            query.Match(
        //                data => {
        //                    var analytics = PeaksAnalytic.Create(data);
        //                    //unitOfWork.
        //                    builder.WithPeaksAnalytic(PeaksAnalytic.Create(data));
        //                },
        //                error => { }
        //            );
        //        },
        //        error => Console.WriteLine($"Failed to create peak analytics: {error.Message}")
        //    );

    }
}
