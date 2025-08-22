using Application.Trips.GpxFile.Services;
using Domain.Common.Geography.ValueObjects;
using System.Globalization;
using System.Xml.Linq;

namespace Infrastructure.Commons.Parsers;

public class GpxParser : IGpxParser {
    public async Task<AnalyticData> ParseAsync(Stream stream) {
        using var reader = new StreamReader(stream);
        var xml = await reader.ReadToEndAsync();

        var doc = XDocument.Parse(xml);
        XNamespace ns = doc.Root.GetDefaultNamespace(); // grabs the default namespace from the root

        var result = doc.Descendants(ns + "trkpt")
            .Select(pt => new GpxPoint(
                double.Parse(pt.Attribute("lat")?.Value ?? "0", CultureInfo.InvariantCulture),
                double.Parse(pt.Attribute("lon")?.Value ?? "0", CultureInfo.InvariantCulture),
                double.Parse(pt.Element(ns + "ele")?.Value ?? "0", CultureInfo.InvariantCulture),
                ParseUtc(pt.Element(ns + "time")?.Value)
            ))
            .ToList();

        if (result == null) {
            throw new Exception("something went wrong during parsing");
        }

        return new(result);
    }

    static DateTime? ParseUtc(string? input) {
        return DateTime.TryParse(
            input,
            CultureInfo.InvariantCulture,
            DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal,
            out var parsed
        )
            ? parsed
            : null;
    }

}
