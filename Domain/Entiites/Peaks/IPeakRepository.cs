namespace Domain.Entiites.Peaks;
public interface IPeakRepository : IReadRepository<Peak, int> {
    Task<bool> AddAsync(Peak peak);
}

