using Application.Services.Files;
using Domain.Common;
using Domain.Common.Result;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Storage;

public class FileStorage(string root = "wwwroot", string subFolder = "uploads") : IGpxFileStorage {
    public readonly string basePath = Path.Combine(root, subFolder);

    public Task<bool> Delete(string path) {
        throw new NotImplementedException();
    }

    public Task<IFormFile> Get(string path) {
        throw new NotImplementedException();
    }

    public async Task<Result<FileCreationInfo>> Save(IFormFile file, string? folderPath) {
        try {
            var uploadsDir = folderPath != null ? Path.Combine(basePath, folderPath) : basePath;
            Directory.CreateDirectory(uploadsDir);

            Console.WriteLine("Saving file in location: " + uploadsDir);
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var fullPath = Path.Combine(uploadsDir, fileName);

            using var stream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(stream);

            FileCreationInfo info = new(fullPath, fileName);
            return Result<FileCreationInfo>.Success(info);
        }
        catch (Exception ex) {
            var error = Errors.Unknown($"Could not save file: {ex.Message}");
            return Result<FileCreationInfo>.Failure(error);
        }
    }

    public Task<bool> Update(string path, IFormFile file) {
        throw new NotImplementedException();
    }
}
