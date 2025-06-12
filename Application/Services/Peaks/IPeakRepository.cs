using Domain;
using Domain.Entiites.Peaks;

namespace Application.Services.Peaks;
public interface IPeakRepository : IReadRepository<Peak, int> {

}