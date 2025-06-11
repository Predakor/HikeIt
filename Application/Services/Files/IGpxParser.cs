using Domain.Trips.ValueObjects;

namespace Application.Services.Files;

public interface IGpxParser {
    Task<AnalyticData> ParseAsync(Stream stream);
}