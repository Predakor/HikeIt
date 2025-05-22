namespace Domain.Peaks;
public interface IPeakRepository {
    Task<IEnumerable<Peak>> GetAllAsync();
    Task<Peak?> GetByIdAsync(int id);
    Task AddAsync(Peak peak);
}

