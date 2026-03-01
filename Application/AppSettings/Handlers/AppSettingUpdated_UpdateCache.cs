using Application.AppSettings.Decorators;
using Application.Commons.Abstractions;
using Domain.AppSettings.Root;

namespace Application.AppSettings.Handlers;

internal sealed class AppSettingUpdated_UpdateCache : IDomainEventHandler<AppSettingEvents.JsonValueUpdated>
{
    private readonly ICache cache;

    public AppSettingUpdated_UpdateCache(ICache cache)
    {
        this.cache = cache;
    }
    public async Task Handle(AppSettingEvents.JsonValueUpdated domainEvent, CancellationToken cancellationToken = default)
    {
        await Task.WhenAll(
           cache.RemoveAsync(CachedAppSettingsServiceDecorator.GetKey(domainEvent.SettingFor), cancellationToken),
           cache.RemoveAsync(CachedAppSettingsServiceDecorator.GetAllKey, cancellationToken)
        );
    }
}
