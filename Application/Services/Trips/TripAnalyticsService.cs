namespace Application.Services.Trips;
public interface ITripAnalyticsService {
    Task Analyze(Guid GpxFileId);

}

public class TripAnalyticsService : ITripAnalyticsService {
    public Task Analyze(Guid GpxFileId) {
        throw new NotImplementedException();
    }
}

