using Application.Dto;
using Domain.Common;
using Domain.Common.Interfaces;
using Domain.Common.Result;
using Domain.Trips.ValueObjects;

namespace Application.TripAnalytics.Commands;

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
