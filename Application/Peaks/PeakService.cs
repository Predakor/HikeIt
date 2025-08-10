using Application.Dto;
using Domain.Common.Factories;
using Domain.Common.Result;
using Domain.Peaks;
using Domain.Trips.ValueObjects;

namespace Application.Peaks;

public class PeakService : IPeaksService {
    readonly IPeaksRepository _repository;

    public PeakService(IPeaksRepository repository) {
        _repository = repository;
    }

    public async Task<Result<Peak>> Add(PeakDto.CreateNew newPeak) {
        IGeoPoint location = new GpxPoint(newPeak.Latitude, newPeak.Longitude, newPeak.Height);

        var peak = Peak.Create(newPeak.Name, location, newPeak.RegionId);

        return await _repository.AddAsync(peak).BindAsync(SaveChanges);
    }

    public async Task<Result<Peak>> Update(int peakId, PeakDto.Update changes) {
        return await _repository.GetByIdAsync(peakId).MapAsync(peak => peak.ApplyChanges(changes));
    }

    async Task<Result<Peak>> SaveChanges(Peak peak) {
        return await _repository.SaveChangesAsync().MapAsync(_ => peak);
    }
}

static class Helpers {
    public static Peak ApplyChanges(this Peak peak, PeakDto.Update changes) {
        if (changes.Height.HasValue) {
            peak.Height = changes.Height.Value;
        }

        if (changes.Name is not null) {
            peak.Name = changes.Name;
        }

        if (changes.RegionId is not null) {
            peak.RegionID = changes.RegionId.Value;
        }

        var lat = peak.Location.X;
        var lon = peak.Location.Y;

        if (changes.Latitude.HasValue) {
            lat = changes.Latitude.Value;
        }
        if (changes.Longitude.HasValue) {
            lon = changes.Longitude.Value;
        }

        if (lat != peak.Location.X || lon != peak.Location.Y) {
            peak.Location = GeoFactory.CreatePoint(lat, lon);
        }

        return peak;
    }
}
