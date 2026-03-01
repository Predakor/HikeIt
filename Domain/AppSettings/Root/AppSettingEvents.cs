using Domain.Common.Abstractions;

namespace Domain.AppSettings.Root;

public abstract record AppSettingEvents
{
    public sealed record JsonValueUpdated(AppSettingType SettingFor) : IDomainEvent;
}
