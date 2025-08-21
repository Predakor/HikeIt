using Domain.FileReferences;
using Domain.Interfaces;

namespace Domain.Trips.Entities.GpxFiles;

public interface IGpxFileRepository : ICrudRepository<FileReference, Guid> {
    Task<FileReference?> GetGpxFile(Guid id);
    Task<Stream?> GetGpxFileStream(Guid id);
}
