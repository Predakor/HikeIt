using Domain.AppSettings.Root;

namespace Domain.AppSettings.Interfaces;

public interface IAppSetting
{
    static abstract AppSettingType SettingFor { get; }

    // Error messages for every invalid field; empty when the setting is valid. Checked before the setting is stored.
    IEnumerable<string> Validate() => [];
};

public interface IAppSetting<TSelf> : IAppSetting
    where TSelf : IAppSetting<TSelf>
{
    static abstract TSelf GetDefault();
}