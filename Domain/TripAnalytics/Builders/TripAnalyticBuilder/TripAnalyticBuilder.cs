namespace Domain.TripAnalytics.Builders.TripAnalyticBuilder;
internal class TripAnalyticBuilder {
}
//class chuj {
//    public TripAnalyticBuilder WithClimbedPeaks() {
//        _peaks = TripBuilderMethods
//            .FindLocalPeaks(_points, _gains)
//            .Select(p => new ReachedPeak() { GpxPoint = p, TimeReached = p?.Time })
//            .ToList();

//        return this;
//    }
//    public TripAnalyticBuilder WithTimeAnalytic() {
//        TripAnalytic tripAnalytic = Build();

//        DateTime start = (DateTime)_points.First().Time!;
//        DateTime end = (DateTime)_points.Last().Time!;

//        TimeFrame timeFrame = new(start, end);

//        List<GpxGainWithTime> gainsWithTime = _gains
//            .Where(g => g.TimeDelta != null)
//            .Select(g => {
//                var timeDelta = (double)g.TimeDelta!;
//                return g.ToGainWithTime(timeDelta);
//            })
//            .ToList();

//        TimeAnalyticData timeAnalyticData = new(tripAnalytic, gainsWithTime, timeFrame);

//        _timeAnalytic = TimeAnalyticsDirector.Create(timeAnalyticData);

//        return this;
//    }
//}