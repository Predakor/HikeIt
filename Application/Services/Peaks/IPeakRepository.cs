using Domain.Entiites.Peaks;
using Domain.Interfaces;

namespace Application.Services.Peaks;
public interface IPeakRepository : IReadRepository<Peak, int> {

}