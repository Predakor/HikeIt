using Domain.Common.Geography.Factories;
using Domain.Common.Geography.ValueObjects;
using Domain.Locations.Peaks;

namespace Application.Locations.Peaks;

public class PeakService : IPeaksService {
    readonly IPeaksRepository _repository;
    readonly IPeaksQueryService _queries;

    public PeakService(IPeaksRepository repository, IPeaksQueryService queries) {
        _repository = repository;
        _queries = queries;
    }

    public async Task<Result<Peak>> Add(PeakDto.CreateNew newPeak) {
        var existingPeak = await _queries.GetPeakWithNameFromRegion(newPeak.Name, newPeak.RegionId);
        if (existingPeak.IsSuccess) {
            return Errors.NotUnique("peak with that name and region already exists");
        }

        IGeoPoint location = new GpxPoint(newPeak.Lat, newPeak.Lon, newPeak.Height);

        var peak = Peak.Create(newPeak.Name, location, newPeak.RegionId);

        return await _repository.AddAsync(peak).BindAsync(SaveChanges);
    }

    public async Task<Result<Peak>> Update(int peakId, PeakDto.Update changes) {
        return await _repository
            .GetByIdAsync(peakId)
            .MapAsync(peak => peak.ApplyChanges(changes))
            .BindAsync(SaveChanges);
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

        var lat = peak.Location.Y;
        var lon = peak.Location.X;

        if (changes.Lat.HasValue) {
            lat = changes.Lat.Value;
        }
        if (changes.Lon.HasValue) {
            lon = changes.Lon.Value;
        }


        if (lat != peak.Location.X || lon != peak.Location.Y) {
            Console.WriteLine("updating peak");
            peak.Location = GeoFactory.CreatePoint(lat, lon);
        }

        return peak;
    }
}
