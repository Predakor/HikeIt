using Application.Commons.Abstractions;
using Application.FileReferences;
using Domain.Common.Result;
using Domain.FileReferences.ValueObjects;

namespace Infrastructure.Commons.Storage.Decorators;

internal class CachedFileStorageDecorator : IFileStorage, IDecorator<IFileStorage> {
    readonly IFileStorage _inner;
    readonly ICache _cache;

    public CachedFileStorageDecorator(IFileStorage inner, ICache cacheService) {
        _cache = cacheService;
        _inner = inner;
    }

    public async Task<Result<Stream>> DownloadAsync(string path, BlobContainer type) {
        return await _cache
            .GetAsync<byte[]>(path)
            .MatchAsync(
                cacheHit => new MemoryStream(cacheHit),
                cacheMiss => DownloadAndCache(path, type)
            );
    }

    public Task<Result<bool>> DeleteAsync(string path, BlobContainer type) {
        return _inner.DeleteAsync(path, type);
    }

    public Task<Result<SaveFileResponse>> UploadAsync(
        FileContent file,
        string path,
        BlobContainer type
    ) {
        return _inner.UploadAsync(file, path, type);
    }

    async Task<Result<Stream>> DownloadAndCache(string path, BlobContainer type) {
        var result = await _inner.DownloadAsync(path, type);

        if (!result.IsSuccess) {
            return result;
        }
        Console.WriteLine("Loading to cache");
        var ms = new MemoryStream();
        await result.Value!.CopyToAsync(ms);
        await _cache.SetAsync(path, ms.ToArray());
        ms.Position = 0;
        return ms;
    }
}
