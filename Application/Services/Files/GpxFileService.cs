using Domain.GpxFiles;
using Domain.Users;
using Microsoft.AspNetCore.Http;

namespace Application.Services.Files;

public class GpxFileService(IFileStorage storage, IGpxFileRepository repository) : IGpxFileService {
    readonly IFileStorage _fileStorage = storage;
    readonly IGpxFileRepository _repository = repository;

    public async Task<bool> CreateAsync(IFormFile file) {
        var (isValid, errors) = Validate(file);
        if (!isValid) {
            //TODO add result class
            Console.WriteLine(errors);
            return false;
        }

        User? user = null;
        string userId = user?.Id.ToString();

        var result = await _fileStorage.Save(file, userId);

        if (result.HasErrors(out var error)) {
            Console.WriteLine(error.Message);
            return false;
        }

        FileCreationInfo info = result.Value!;

        GpxFile entity = new() {
            Name = info.Name,
            Path = info.Path,
            OwnerId = user?.Id,
        };

        await _repository.AddAsync(entity);
        return await _repository.SaveChangesAsync();

    }

    public Task<bool> DeleteAsync(int id) {
        throw new NotImplementedException();
    }

    public Task<GpxFileDto> GetByIdAsync(int id) {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateAsync(string path, IFormFile file) {
        throw new NotImplementedException();
    }

    static (bool isValid, List<string> errors) Validate(IFormFile? file) {
        const int maxSizeInBytes = 1024 * 1024 / 2; // 0.5 MB
        const string allowedExtension = ".gpx";

        var errors = new List<string>();

        if (file == null || file.Length == 0) {
            errors.Add("No file uploaded.");
            return (false, errors); // Return immediately here; can't proceed with other checks
        }

        if (!file.FileName.EndsWith(allowedExtension, StringComparison.OrdinalIgnoreCase)) {
            errors.Add($"Only {allowedExtension} files are allowed.");
        }

        if (file.Length > maxSizeInBytes) {
            double maxSizeInMB = maxSizeInBytes / 1024f / 1024f;
            errors.Add($"File is too large. Max size: {maxSizeInMB:F1} MB.");
        }

        return (errors.Count == 0, errors);
    }
}
