using Domain.Common;
using Domain.Entiites.Users;
using Domain.Trips.GpxFiles;
using Microsoft.AspNetCore.Http;

namespace Application.Services.Files;

public class GpxFileService(IFileStorage storage, IGpxFileRepository repository) : IGpxFileService {
    readonly IFileStorage _fileStorage = storage;
    readonly IGpxFileRepository _repository = repository;

    public async Task<Result<GpxFile>> CreateAsync(IFormFile file) {
        var (isValid, errors) = Validate(file);
        if (!isValid) {
            var err = new Error("multiple validation errors", errors.ToString());
            return Result<GpxFile>.Failure(err);
        }

        User? user = null;
        string userId = user?.Id.ToString();

        var result = await _fileStorage.Save(file, userId);

        if (result.HasErrors(out var error)) {
            Console.WriteLine(error.Message);
            return Result<GpxFile>.Failure(new("saving", error.Message));
        }

        FileCreationInfo info = result.Value!;
        Guid id = Guid.NewGuid();
        GpxFile entity = new() {
            Id = id,
            Name = info.Name,
            Path = info.Path,
            OwnerId = user?.Id,
        };

        await _repository.AddAsync(entity);
        var succes = await _repository.SaveChangesAsync();

        if (!succes) {
            return Result<GpxFile>.Failure(new("err", "couldn't save"));
        }
        return Result<GpxFile>.Success(entity);
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

    static (bool isValid, List<Error> errors) Validate(IFormFile? file) {
        const int maxSizeInBytes = 1024 * 1024 / 2; // 0.5 MB
        const string allowedExtension = ".gpx";

        var errors = new List<Error>();

        void AddError(string code, string message) {
            errors.Add(new Error(code, message));
        }

        if (file == null || file.Length == 0) {
            AddError("nullRef", "No file uploaded.");
            return (false, errors); // Return immediately here; can't proceed with other checks
        }

        if (!file.FileName.EndsWith(allowedExtension, StringComparison.OrdinalIgnoreCase)) {
            AddError("Invalid extention", $"Only {allowedExtension} files are allowed.");
        }

        if (file.Length > maxSizeInBytes) {
            double maxSizeInMB = maxSizeInBytes / 1024f / 1024f;
            AddError("File size", $"File is too large. Max size: {maxSizeInMB:F1} MB.");
        }

        return (errors.Count == 0, errors);
    }
}
