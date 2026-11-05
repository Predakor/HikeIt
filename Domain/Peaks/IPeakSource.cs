using Domain.Common.Geography.ValueObjects;

namespace Domain.Peaks;

public interface IPeakSource
{
    Task<List<Peak>> GetPeaksInBounds(GeoBounds bounds);
}