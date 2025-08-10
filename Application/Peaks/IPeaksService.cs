using Application.Dto;
using Domain.Common.Result;
using Domain.Peaks;

namespace Application.Peaks;

public interface IPeaksService {
    Task<Result<Peak>> Add(PeakDto.CreateNew newPeak);
    Task<Result<Peak>> Update(int peakId, PeakDto.Update newPeak);
}
