using Domain.Common.Result;
using Domain.Entiites.Peaks;
using Domain.Interfaces;

namespace Domain.Entiites.Regions;
public interface IRegionRepository : IReadResultRepository<Region, int> {
    Task<Result<List<Peak>>> AllPeaksFromRegion(Region region);
}
