using Application.Interfaces;
using Application.Services.Files;
using Domain.Trips.Events;

namespace Application.Trips.GpxFile;

internal class TripDeleted_RemoveGpxFileHandler : IDomainEventHandler<TripRemovedEvent> {
    readonly IGpxFileService _fileService;

    public TripDeleted_RemoveGpxFileHandler(IGpxFileService fileService) {
        _fileService = fileService;
    }

    public async Task Handle(
        TripRemovedEvent domainEvent,
        CancellationToken cancellationToken = default
    ) {
        var filePath = domainEvent?.Trip?.GpxFile?.Name;
        if (filePath is null) {
            Console.WriteLine("No path to file can't delete");
            await Task.CompletedTask;
            return;
        }

        await _fileService.DeleteAsync(filePath);
    }
}
