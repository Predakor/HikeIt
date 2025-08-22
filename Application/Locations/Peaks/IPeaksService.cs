using Domain.Common.Result;
using Domain.Locations.Peaks;

namespace Application.Locations.Peaks;

public interface IPeaksService {
    Task<Result<Peak>> Add(PeakDto.CreateNew newPeak);
    Task<Result<Peak>> Update(int peakId, PeakDto.Update newPeak);
}
