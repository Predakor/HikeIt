using Domain.Common.Result;
using Domain.Common.ValueObjects;
using Microsoft.AspNetCore.Http;

namespace Application.Commons.FileStorage;
public interface IFileStorage {
    Task<Result<Stream>> DownloadAsync(Guid id);
    Task<Result<FileReference>> UploadAsync(IFormFile file, Guid id);
    Task<Result<bool>> DeleteAsync(Guid id);
    Task<Result<FileReference>> UpdateAsync(Guid id);
    Task<Result<string>> GetFileUrlAsync(Guid id);

}
