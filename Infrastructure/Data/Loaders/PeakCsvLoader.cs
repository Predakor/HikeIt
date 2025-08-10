using CsvHelper;
using Domain.Common.Factories;
using Domain.Mountains.Regions;
using Domain.Peaks;
using Infrastructure.Data.Seeding;
using System.Collections.Immutable;
using System.Globalization;

namespace Infrastructure.Data.Loaders;

record PeakCsvEntry(string Name, int Height, string Range, double Lat, double Lon);

internal class PeakCsvLoader {
    static T[] ExtractData<T>(string stringStream) {
        var csvConfig = new CsvHelper.Configuration.CsvConfiguration(CultureInfo.InvariantCulture) {
            PrepareHeaderForMatch = args => args.Header.ToLower(),
        };

        using var reader = new StringReader(stringStream);
        using var csv = new CsvReader(reader, csvConfig);

        return csv.GetRecords<T>().ToArray();
    }

    public static async Task<ImmutableArray<Peak>> LoadFromLink(string link) {
        HttpClient client = new();
        var response = await client.GetAsync(link);

        if (!response.IsSuccessStatusCode) {
            throw new Exception("failed to fetch data required for seeding terminating app");
        }

        var csv =
            await response.Content.ReadAsStringAsync()
            ?? throw new Exception("failed to fetch data required for seeding terminating app");

        return ExtractData<PeakCsvEntry>(csv).MapToPeaks(DataSeed.Regions);
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
        return new Peak() {
            Name = r.Name,
            Height = r.Height,
            Location = GeoFactory.CreatePoint(r.Lon, r.Lat),
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
