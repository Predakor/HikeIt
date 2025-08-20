using Application.Commons.CacheService;
using Application.Commons.FileStorage;
using Domain.Common;
using Domain.Common.Result;
using Domain.FileReferences;
using Domain.FileReferences.ValueObjects;
using Domain.Trips.Entities.GpxFiles;
using Domain.Trips.ValueObjects;
using Microsoft.AspNetCore.Http;

namespace Application.Services.Files;

public class GpxFileService : IGpxFileService {
    const BlobContainer container = BlobContainer.File;

    readonly IGpxFileRepository _repository;
    readonly IFileStorage _storage;
    readonly ICacheService _cache;
    readonly IGpxParser _parser;

    public GpxFileService(
        IGpxFileRepository repository,
        ICacheService cacheService,
        IFileStorage storage,
        IGpxParser parser
    ) {
        _repository = repository;
        _cache = cacheService;
        _storage = storage;
        _parser = parser;
    }

    static string FileKey(Guid fileId, Guid userId) => $"{userId}/{fileId}";

    public async Task<Result<FileReference>> CreateAsync(FileContent file, Guid userId, Guid tripId) {
        string key = FileKey(tripId, userId);

        return await _cache
            .SetAsync(key, file)
            .MapAsync(info => FileReference.FromFileContent(file, key, tripId));
    }

    public async Task<Result<AnalyticData>> ExtractGpxData(FileContent file) {
        try {
            using var ms = new MemoryStream(file.Content);
            return await _parser.ParseAsync(ms);
        }
        catch (Exception ex) {
            return Errors.Unknown(ex.Message);
        }
    }

    public async Task<Result<AnalyticData>> ExtractGpxData(Guid id) {
        var result = await _repository.GetGpxFileStream(id);
        if (result == null) {
            return Errors.NotFound("No file with id found");
        }

        var data = await _parser.ParseAsync(result);
        if (data == null) {
            return Errors.Unknown("something went wrong");
        }

        return data;
    }

    public Result<IFormFile> Validate(IFormFile file) => FileValidator.ValidateGpx(file);

    public Task<Result<FileContent>> ValidateAndExtract(IFormFile file) {
        return Validate(file).MapAsync(f => f.ToFileContent());
    }

    public async Task<Result<string>> UploadAsync(Guid fileId, Guid userId) {
        string key = FileKey(fileId, userId);
        return await _cache
            .GetAsync<FileContent>(key)
            .BindAsync(file => _storage.UploadAsync(file, key, container))
            .MapAsync(r => r.Url);
    }

    public async Task<Result<bool>> DeleteAsync(string path) {
        return await _storage.DeleteAsync(path, container);
    }
}
