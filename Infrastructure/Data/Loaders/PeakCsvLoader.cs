using CsvHelper;
using Domain.Mountains.Peaks;
using Domain.Mountains.Regions;
using Infrastructure.Data.Seeding;
using NetTopologySuite.Geometries;
using System.Collections.Immutable;
using System.Globalization;

namespace Infrastructure.Data.Loaders;

record PeakCsvEntry(string Name, int Height, string Range, double Lat, double Lon);

internal class PeakCsvLoader {
    static List<T> ExtractData<T>(string path) {
        var csvConfig = new CsvHelper.Configuration.CsvConfiguration(CultureInfo.InvariantCulture) {
            PrepareHeaderForMatch = args => args.Header.ToLower(),
        };

        using var reader = new StreamReader(path);
        using var csv = new CsvReader(reader, csvConfig);

        return [.. csv.GetRecords<T>()];
    }

    public static ImmutableArray<Peak> LoadFrom(string path) {
        return ExtractData<PeakCsvEntry>(path).MapToPeaks(DataSeed.Regions);
    }
}

internal static class CsvExtentions {
    public static ImmutableArray<Peak> MapToPeaks(
        this IEnumerable<PeakCsvEntry> csvEntries,
        IEnumerable<Region> regions
    ) {
        return [.. csvEntries.Select(r => MapToPeak(r, regions))];
    }

    static Peak MapToPeak(PeakCsvEntry r, IEnumerable<Region> regions) {
        const int Srid = 4326;

        return new Peak() {
            Name = r.Name,
            Height = r.Height,
            Location = new Point(r.Lon, r.Lat) { SRID = Srid },
            RegionID = MatchToRegionId(r.Range, regions),
        };
    }

    static int MatchToRegionId(string regionName, IEnumerable<Region> regions) {
        var foundRegion = regions.FirstOrDefault(r => r.Name == regionName);

        return foundRegion != null
            ? foundRegion.Id
            : throw new Exception($"Region '{regionName}' not found in predefined regions.");
    }
}
