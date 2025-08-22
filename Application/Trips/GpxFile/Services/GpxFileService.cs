using Application.Commons.Abstractions;
using Application.FileReferences;
using Domain.FileReferences;
using Domain.FileReferences.ValueObjects;

namespace Application.Trips.GpxFile.Services;

public class GpxFileService : IGpxFileService {
    const BlobContainer container = BlobContainer.File;

    readonly IFileStorage _storage;
    readonly ICache _cache;

    public GpxFileService(ICache cacheService, IFileStorage storage) {
        _cache = cacheService;
        _storage = storage;
    }

    static string FileKey(Guid fileId, Guid userId) => $"{userId}/{fileId}";

    public async Task<Result<Stream>> GetAsync(FileReference reference) {
        return await _storage.DownloadAsync(reference.StorageName, container);
    }

    public async Task<Result<string>> UploadAsync(Guid fileId, Guid userId) {
        string key = FileKey(fileId, userId);
        return await _cache
            .GetAsync<FileContent>(key)
            .BindAsync(file => _storage.UploadAsync(file, key, container))
            .MapAsync(r => r.Url);
    }

    public async Task<Result<FileReference>> UploadAssync(FileReference reference) {
        return await _cache
            .GetAsync<FileContent>(reference.StorageName)
            .BindAsync(file => _storage.UploadAsync(file, reference.StorageName, container))
            .MapAsync(r => reference.SetUrl(r.Url));
    }

    public async Task<Result<bool>> DeleteAsync(string path) {
        return await _storage.DeleteAsync(path, container);
    }

    public async Task<Result<FileReference>> CreateTemporrary(
        FileContent file,
        Guid userId,
        Guid tripId
    ) {
        string key = FileKey(tripId, userId);

        return await _cache
            .SetAsync(key, file)
            .MapAsync(info => FileReference.FromFileContent(file, key, tripId));
    }
}
