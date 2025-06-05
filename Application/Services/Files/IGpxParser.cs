using Domain.Trips.ValueObjects;

namespace Application.Services.Files;

public interface IGpxParser {
    Task<GpxAnalyticData> ParseAsync(Stream stream);
}