using Domain.Common.Abstractions;

namespace Domain.AppSettings.Root;
public abstract record AppSettingEvents
{
    public sealed record JsonValueUpdated(int SettingId) : IDomainEvent;
}
