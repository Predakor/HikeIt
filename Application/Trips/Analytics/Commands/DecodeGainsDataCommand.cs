using Application.Trips.Analytics.ElevationProfiles;
using Domain.Common.Abstractions;
using Domain.Common.Geography;
using Domain.Common.Geography.ValueObjects;

namespace Application.Trips.Analytics.Commands;

internal class DecodeGainsDataCommand(byte[] encodedGains) : ICommand<GainDto[]> {
    public Result<GainDto[]> Execute() {
        var scaledGains = ScaledGainSerializer.Deserialize(encodedGains);

        static GainDto FromScaledGain(ScaledGain scaledGain) {
            return new GainDto(
                scaledGain.DistanceDelta,
                scaledGain.ElevationDelta,
                scaledGain.TimeDelta
            );
        }

        return scaledGains.Select(FromScaledGain).ToArray();

    }

    public static ICommand<GainDto[]> Create(byte[] encodedGains) {
        return new DecodeGainsDataCommand(encodedGains);
    }
}
