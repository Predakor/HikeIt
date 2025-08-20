using Domain.Common.Interfaces;
using Domain.Common.Result;
using Domain.Trips.Builders.GpxDataBuilder;
using Domain.Trips.Config;
using Domain.Trips.ValueObjects;

namespace Domain.TripAnalytics.Commands;

public class CreateElevationProfileDataCommand : ICommand<AnalyticData> {
    readonly AnalyticData _data;
    readonly DataProccesConfig _config;

    public CreateElevationProfileDataCommand(AnalyticData data, DataProccesConfig config) {
        _data = data;
        _config = config;
    }

    public Result<AnalyticData> Execute() {
        var dataWithConfig = new ElevationDataWithConfig(_data, _config);
        return GpxDataFactory.CreateFromConfig(dataWithConfig);
    }

    public static ICommand<AnalyticData> Create(AnalyticData analyticData, DataProccesConfig config) {
        return new CreateElevationProfileDataCommand(analyticData, config);
    }
}
