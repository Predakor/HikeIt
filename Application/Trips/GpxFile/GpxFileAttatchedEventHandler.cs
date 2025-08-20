using Application.Interfaces;
using Application.Services.Files;
using Domain.Common.Result;
using Domain.Trips;
using Domain.Trips.Events;

namespace Application.Trips.GpxFile;

internal class GpxFileAttatchedEventHandler : IDomainEventHandler<GpxFileAttatchedEvent> {
    readonly IGpxFileService _fileService;
    readonly ITripRepository _repository;

    public GpxFileAttatchedEventHandler(IGpxFileService fileService, ITripRepository repository) {
        _fileService = fileService;
        _repository = repository;
    }

    public async Task Handle(
        GpxFileAttatchedEvent domainEvent,
        CancellationToken cancellationToken = default
    ) {
        (Guid FileId, Guid TripId) = domainEvent;

        var getTrip = await _repository.GetWithFile(FileId);
        if (getTrip.HasErrors(out var error)) {
            Console.WriteLine("Failed to update gpx file: " + error.Message);
            return;
        }

        var trip = getTrip.Value!;

        var file =

        await _fileService
            .UploadAsync(FileId, trip.UserId)
            .TapAsync(fileUrl => {
                if (trip.GpxFile is not null) {
                    trip.GpxFile.Path = fileUrl;
                }
            });
    }
}
