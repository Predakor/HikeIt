using Application.Services.Files;
using Domain.Trips.ValueObjects;
using System.Globalization;
using System.Xml.Linq;

namespace Infrastructure.Parsers;


public class GpxParser : IGpxParser {
    public async Task<GpxAnalyticData> ParseAsync(Stream stream) {
        using var reader = new StreamReader(stream);
        var xml = await reader.ReadToEndAsync();

        var doc = XDocument.Parse(xml);

        var result = doc.Descendants()
            .Where(e => e.Name.LocalName == "trkpt")
            .Select(pt => new GpxPoint(
                double.Parse(pt.Attribute("lat")?.Value ?? "0", CultureInfo.InvariantCulture),
                double.Parse(pt.Attribute("lon")?.Value ?? "0", CultureInfo.InvariantCulture),
                double.Parse(
                    pt.Element(XName.Get("ele"))?.Value ?? "0",
                    CultureInfo.InvariantCulture
                ),
                DateTime.TryParse(pt.Element(XName.Get("time"))?.Value, out var t) ? t : null
            ))
            .ToList();

        if (result == null) {
            throw new Exception("something went wrong during parsing");
        }

        return new(result);
    }

}