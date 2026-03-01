using Domain.AppSettings.Root;

namespace Domain.AppSettings.Interfaces;

public interface IAppSetting
{
    string Name { get; }
    AppSettingType SettingFor { get; }
};