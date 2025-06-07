using Domain.Trips.ValueObjects;

namespace Application.Services.Files;

public interface IGpxParser {
    Task<TripAnalyticData> ParseAsync(Stream stream);
}