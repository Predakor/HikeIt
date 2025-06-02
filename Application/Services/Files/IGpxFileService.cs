using Domain.Common;
using Domain.Trips.GpxFiles;
using Microsoft.AspNetCore.Http;

namespace Application.Services.Files;
public interface IGpxFileService {
    Task<GpxFileDto> GetByIdAsync(int id);
    Task<Result<GpxFile>> CreateAsync(IFormFile file);
    Task<bool> UpdateAsync(string path, IFormFile file);
    Task<bool> DeleteAsync(int id);
}

public abstract record GpxFileDto {

    public record Request(string Path) : GpxFileDto;
    public record New(string Path) : GpxFileDto;

}