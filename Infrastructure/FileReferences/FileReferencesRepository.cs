using Domain.FileReferences;
using Infrastructure.Data;
using Infrastructure.Repository.Generic;

namespace Infrastructure.FileReferences;

internal class FileReferencesRepository
    : ResultRepository<FileReference, Guid>,
        IFileReferenceRepository {
    public FileReferencesRepository(TripDbContext context)
        : base(context) { }
}
