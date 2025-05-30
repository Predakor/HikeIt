using Domain.Common;
using Microsoft.AspNetCore.Http;

namespace Application.Services.Files;
public interface IFileStorage {

    Task<IFormFile> Get(string path);
    Task<Result<FileCreationInfo>> Save(IFormFile file, string folderPath);
    Task<bool> Update(string path, IFormFile file);
    Task<bool> Delete(string path);
}


public record FileCreationInfo(string Path, string Name);