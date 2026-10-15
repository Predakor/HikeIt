using System.Security.Cryptography;
using Domain.Trips.Root.Builders.GpxDataBuilder;

namespace Domain.Tests.Pipeline;

// Pins the production cleanup output (GpxDataDirector.AnalyticData) to hashes captured before the
// filters became the single implementation. Elevations are hashed by their exact bits; Lat/Lon must be untouched.
public class CleanupGoldenTests
{
    public static TheoryData<string, string, string> GoldenHashes { get; } = new()
    {
        {
            "data/trip_small.gpx",
            "44E2BC7A88855CE58B45A6F6427E66A729A82B8CC2672BD5A73CD3299C66D69D",
            "3C427589C406FC4DAAA8664A9AB3BF49A6E6588B362ED9AA0CA33CF69D1AEDFF"
        },
        {
            "data/trip_medium.gpx",
            "DF5346258499BB29E51CB67B21B18D8E6E34FF7477CEF6E34A3E4E9B475CC2B0",
            "4AFD48385F1EE44871BF309F2E477DD3723B585A5E0F257006C0AC956CFD6670"
        },
        {
            "data/trip_big.gpx",
            "D4B5652924ADE90625DC463F716782FDD595EF99B36E22BDE84626C7A1F89917",
            "82E0B66FA807C22F159C7C3A7229765F49CC24944B3F9CA8EAF98533D9A16248"
        },
    };

    [Theory]
    [MemberData(nameof(GoldenHashes))]
    public async Task CleanupOutput_MatchesGoldenHashes(string path, string expectedEleHash, string expectedLatLonHash)
    {
        var data = await ParserTests.ParseFromGpxFile(path);

        var output = GpxDataDirector.AnalyticData(data.Points).Points;

        var actual = (
            EleHash: Hash(output.Select(p => p.Ele)),
            LatLonHash: Hash(output.SelectMany(p => new[] { p.Lat, p.Lon }))
        );
        Assert.Equal((expectedEleHash, expectedLatLonHash), actual);
    }

    static string Hash(IEnumerable<double> values)
    {
        using var stream = new MemoryStream();
        foreach (var value in values)
        {
            stream.Write(BitConverter.GetBytes(BitConverter.DoubleToInt64Bits(value)));
        }

        return Convert.ToHexString(SHA256.HashData(stream.ToArray()));
    }
}
