using Domain.Common.Abstractions;
using Domain.Common.Geography.ValueObjects;
using Domain.Common.Result;
using Domain.Trips.Root.Builders.Config;
using Domain.Trips.Root.Builders.GpxDataBuilder;

namespace Domain.Trips.Analytics.ElevationProfiles.Commands;

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
