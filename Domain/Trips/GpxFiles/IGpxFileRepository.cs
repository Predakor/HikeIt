namespace Domain.Trips.GpxFiles;
public interface IGpxFileRepository : ICrudRepository<GpxFile, Guid> {
    Task<GpxFile?> GetGpxFile(Guid id);
}
