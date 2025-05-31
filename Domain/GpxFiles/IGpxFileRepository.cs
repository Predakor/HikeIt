namespace Domain.GpxFiles;
public interface IGpxFileRepository : ICrudRepository<GpxFile> {
    Task<GpxFile?> GetGpxFile(Guid id);
}
