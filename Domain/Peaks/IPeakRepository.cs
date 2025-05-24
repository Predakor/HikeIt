namespace Domain.Peaks;
public interface IPeakRepository : IReadRepository<Peak> {
    Task<bool> AddAsync(Peak peak);
}

