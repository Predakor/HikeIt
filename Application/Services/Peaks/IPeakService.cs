using Application.Dto;
namespace Application.Services.Peaks;

public interface IPeakService {
    Task<IEnumerable<PeakDto.Complete>> GetAllPeaksAsync();
    Task<PeakDto.Complete> GetPeakByIdAsync(int id);
    Task CreatePeakAsync(PeakDto.Simple peakDto);
}
