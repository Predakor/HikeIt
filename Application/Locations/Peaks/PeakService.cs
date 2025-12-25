using Domain.Common.Geography.Factories;
using Domain.Common.Geography.ValueObjects;
using Domain.Locations.Peaks;
using Domain.Peaks;

namespace Application.Locations.Peaks;

public class PeakService : IPeaksService
{
    private readonly IPeaksRepository _repository;
    private readonly IPeaksQueryService _queries;
    private readonly TimeProvider _timeProvider;

    public PeakService(IPeaksRepository repository, IPeaksQueryService queries, TimeProvider timeProvider)
    {
        _repository = repository;
        _queries = queries;
        _timeProvider = timeProvider;
    }

    public async Task<Result<Peak>> Add(PeakDto.CreateNew newPeak)
    {
        var existingPeak = await _queries.GetPeakWithNameFromRegion(newPeak.Name, newPeak.RegionId);
        if (existingPeak.IsSuccess)
        {
            return Errors.NotUnique("peak with that name and region already exists");
        }

        IGeoPoint location = new GpxPoint(newPeak.Lat, newPeak.Lon, newPeak.Height);

        var peak = Peak.Create(newPeak.Name, location, newPeak.RegionId);

        return await _repository.AddAsync(peak).BindAsync(SaveChanges);
    }

    public async Task<Result<Peak>> Update(int peakId, PeakDto.Update newPeak)
    {
        return await _repository
            .GetByIdAsync(peakId)
            .MapAsync(peak => peak.ApplyChanges(newPeak))
            .BindAsync(SaveChanges);
    }

    public async Task<Result<Peak>> Delete(int peakId)
    {
        return await _repository
            .GetByIdAsync(peakId)
            .MapAsync(x => x.Delete(_timeProvider.GetUtcNow()));
    }

    private async Task<Result<Peak>> SaveChanges(Peak peak)
    {
        return await _repository.SaveChangesAsync().MapAsync(_ => peak);
    }
}

internal static class Helpers
{
    public static Peak ApplyChanges(this Peak peak, PeakDto.Update changes)
    {
        if (changes.Height.HasValue)
        {
            peak.Height = changes.Height.Value;
        }

        if (changes.Name is not null)
        {
            peak.Name = changes.Name;
        }

        if (changes.RegionId is not null && changes.RegionId != peak.RegionID)
        {
            peak.RegionID = changes.RegionId.Value;
        }

        var lat = peak.Location.Y;
        var lon = peak.Location.X;

        if (changes.Lat.HasValue)
        {
            lat = changes.Lat.Value;
        }

        if (changes.Lon.HasValue)
        {
            lon = changes.Lon.Value;
        }

        if (lat != peak.Location.X || lon != peak.Location.Y)
        {
            Console.WriteLine("updating peak");
            peak.Location = GeoFactory.CreatePoint(lat, lon);
        }

        return peak;
    }
}
