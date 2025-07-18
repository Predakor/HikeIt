using CsvHelper;
using Domain.Entiites.Peaks;
using Domain.Entiites.Regions;
using NetTopologySuite.Geometries;
using System.Globalization;

namespace Infrastructure.Data.Seeding;

internal record PeakCsvEntry(string Name, int Height, string Range, double Lat, double Lon);

public partial class Seeder {
    public void InsertMountainPeaks(TripDbContext dbContext, string filePath) {
        // Already seeded
        if (dbContext.Peaks.Any()) {
            return;
        }

        var peaks = CsvExtentions.ExtractData<PeakCsvEntry>(filePath).MapToPeaks(DataSeed.Regions);

        if (peaks.Count == 0) {
            throw new Exception("No mountain peaks found in the CSV file.");
        }

        dbContext.Peaks.AddRange(peaks);
        dbContext.SaveChanges();
    }
}

internal static class CsvExtentions {
    public static List<Peak> MapToPeaks(
        this IEnumerable<PeakCsvEntry> csvEntries,
        IEnumerable<Region> regions
    ) {
        const int srid = 4326;

        return csvEntries
            .Select(r => new Peak() {
                Name = r.Name,
                Height = r.Height,
                Location = new Point(r.Lon, r.Lat) { SRID = srid },
                RegionID = MatchToRegionId(r.Range),
            })
            .ToList();

        int MatchToRegionId(string regionName) {
            var foundRegion = regions.FirstOrDefault(r => r.Name == regionName);

            return foundRegion != null
                ? foundRegion.Id
                : throw new Exception($"Region '{regionName}' not found in predefined regions.");
        }
    }

    public static List<T> ExtractData<T>(string path) {
        var csvConfig = new CsvHelper.Configuration.CsvConfiguration(CultureInfo.InvariantCulture) {
            PrepareHeaderForMatch = args => args.Header.ToLower(),
        };

        using var reader = new StreamReader(path);
        using var csv = new CsvReader(reader, csvConfig);

        return csv.GetRecords<T>().ToList();
    }
}
