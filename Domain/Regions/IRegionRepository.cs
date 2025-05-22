namespace Domain.Regions;
public interface IRegionRepository {
    public Task<Region?> GetAsync(int id);
    public Task<IEnumerable<Region>> GetAllAsync();
}
