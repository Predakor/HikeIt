using Domain.AppSettings.Root;

namespace Domain.AppSettings.Interfaces;

public interface IAppSetting
{
    AppSettingType SettingFor { get; }
};