namespace Domain.Trips.Entities.GpxFiles;
public interface IGpxFileRepository : ICrudRepository<GpxFile, Guid> {
    Task<GpxFile?> GetGpxFile(Guid id);
    Task<Stream?> GetGpxFileStream(Guid id);
}
