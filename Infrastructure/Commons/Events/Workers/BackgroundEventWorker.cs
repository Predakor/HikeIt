using Application.Commons.Abstractions;
using Domain.Common.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Commons.Events.Workers;

internal class BackgroundEventWorker : BackgroundService {
    readonly ILogger<IEvent> _logger;
    readonly IBackgroundQueue _queue;
    readonly IServiceProvider _services;

    public BackgroundEventWorker(
        IBackgroundQueue queue,
        IServiceProvider services,
        ILogger<IEvent> logger
    ) {
        _services = services;
        _logger = logger;
        _queue = queue;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
        while (!stoppingToken.IsCancellationRequested) {
            try {
                var @event = await _queue.DequeueAsync(stoppingToken);
                if (@event is null) {
                    continue;
                }

                _logger.LogDebug("Dispatching: {@event}", @event.ToString());
                using var scope = _services.CreateScope();
                var dispatcher = scope.ServiceProvider.GetRequiredService<IDomainEventDispatcher>();

                await dispatcher.DispatchAsync((IDomainEvent)@event, stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested) {
                // graceful shutdown
            }
            catch (Exception ex) {
                _logger.LogError("{ message}", ex.Message);
            }
        }
    }
}
