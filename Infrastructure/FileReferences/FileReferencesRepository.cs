using Application.Commons.CacheService;
using Domain.Common.Result;
using Domain.FileReferences;
using Infrastructure.Data;
using Infrastructure.Repository.Generic;

namespace Infrastructure.FileReferences;

internal class FileReferencesRepository
    : ResultRepository<FileReference, Guid>,
        IFileReferenceRepository {
    readonly ICache _cache;

    public FileReferencesRepository(TripDbContext context, ICache cache)
        : base(context) {
        _cache = cache;
    }

    public override Task<Result<FileReference>> GetByIdAsync(Guid id) {
        return _cache.GetOrCreateAsync($"file-{id}", () => base.GetByIdAsync(id));
    }
}
