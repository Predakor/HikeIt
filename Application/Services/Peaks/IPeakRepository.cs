using Domain.Common.Result;
using Domain.Entiites.Peaks;
using Domain.Interfaces;
using NetTopologySuite.Geometries;

namespace Application.Services.Peaks;
public interface IPeakRepository : IReadRepository<Peak, int> {
    Task<Result<Peak>> GetPeakWithinRadius(Point point, float radius);
    Task<Result<IList<Peak>>> GetPeaksWithinRadius(IReadOnlyList<Point> points, float radius);

}