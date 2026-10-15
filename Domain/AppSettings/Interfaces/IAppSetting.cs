using Domain.AppSettings.Root;
using System.Text.Json.Serialization;

namespace Domain.AppSettings.Interfaces;

public interface IAppSetting
{
    [JsonIgnore] string Name { get; }
    [JsonIgnore] AppSettingType SettingFor { get; }

    // Error messages for every invalid field; empty when the setting is valid. Checked before the setting is stored.
    IEnumerable<string> Validate() => [];
};