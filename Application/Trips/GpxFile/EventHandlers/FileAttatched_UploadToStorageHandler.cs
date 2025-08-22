using Application.Commons.Abstractions;
using Application.FileReferences;
using Domain.Common.Result;
using Domain.Trips.Root;
using Domain.Trips.Root.Events;
using Microsoft.Extensions.Logging;

namespace Application.Trips.GpxFile.EventHandlers;

internal class FileAttatched_UploadToStorageHandler : IDomainEventHandler<GpxFileAttatchedEvent> {
    readonly ILogger<GpxFileAttatchedEvent> _logger;
    readonly IGpxFileService _fileService;
    readonly ITripRepository _repository;

    public FileAttatched_UploadToStorageHandler(
        ILogger<GpxFileAttatchedEvent> logger,
        IGpxFileService fileService,
        ITripRepository repository
    ) {
        _fileService = fileService;
        _repository = repository;
        _logger = logger;
    }

    public async Task Handle(
        GpxFileAttatchedEvent domainEvent,
        CancellationToken cancellationToken = default
    ) {
        (Guid FileId, Guid TripId) = domainEvent;

        var getTrip = await _repository.GetWithFile(FileId);
        if (getTrip.HasErrors(out var error)) {
            _logger.LogError("Failed to update gpx file {reason}", error.Message);
            return;
        }

        var trip = getTrip.Value!;

        var gpxFile =
            trip?.GpxFile
            ?? throw new Exception("Gpx file in trip is empty and it shouldn't check your query");

        var file = await _fileService
            .UploadAsync(trip.Id, trip.UserId)
            .TapAsync(fileUrl => trip.GpxFile.SetUrl(fileUrl))
            .BindAsync(_ => _repository.SaveChangesAsync());
    }
}
