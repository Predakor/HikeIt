using Domain.AppSettings.Root;
using System.Text.Json.Serialization;

namespace Domain.AppSettings.Interfaces;

public interface IAppSetting
{
    [JsonIgnore] string Name { get; }
    [JsonIgnore] AppSettingType SettingFor { get; }
};