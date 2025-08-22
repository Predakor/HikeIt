using Domain.Common.Geography.ValueObjects;

namespace Application.Trips.GpxFile.Services;

public interface IGpxParser {
    Task<AnalyticData> ParseAsync(Stream stream);
}