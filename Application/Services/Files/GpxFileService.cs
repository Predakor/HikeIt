using Domain.Common;
using Domain.Entiites.Users;
using Domain.Trips.Entities.GpxFiles;
using Domain.Trips.ValueObjects;
using Microsoft.AspNetCore.Http;

namespace Application.Services.Files;

public class GpxFileService(IFileStorage storage, IGpxFileRepository repository, IGpxParser parser)
    : IGpxFileService {
    readonly IFileStorage _fileStorage = storage;
    readonly IGpxFileRepository _repository = repository;
    readonly IGpxParser _parser = parser;

    public async Task<Result<GpxFile>> CreateAsync(IFormFile file) {
        var (isValid, errors) = FileValidation.Validate(file);
        if (!isValid) {
            var err = new Error("multiple validation errors", errors.ToString());
            return Result<GpxFile>.Failure(err);
        }

        User? user = null;
        Guid userId = Guid.Parse("7a4f8c5b-19b7-4a6a-89c0-f9a2e98a9380");

        var result = await _fileStorage.Save(file, userId.ToString());

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
            OwnerId = userId,
        };

        await _repository.AddAsync(entity);
        var succes = await _repository.SaveChangesAsync();

        Console.WriteLine(succes);
        Console.WriteLine(entity.Name);
        //if (!succes) {
        //    return Result<GpxFile>.Failure(
        //        Error.Unknown("something went wrong while saving your file")
        //    );
        //}
        return Result<GpxFile>.Success(entity);
    }

    public async Task<AnalyticData> GetGpxDataFromFile(IFormFile file) {
        return await _parser.ParseAsync(file.OpenReadStream());
    }

    public async Task<Result<AnalyticData>> GetGpxDataByFileIdAsync(Guid id) {
        var result = await _repository.GetGpxFileStream(id);
        if (result == null) {
            return Result<AnalyticData>.Failure(Error.NotFound("No file with id found"));
        }

        var data = await _parser.ParseAsync(result);
        if (data == null) {
            return Result<AnalyticData>.Failure(Error.Unknown("something went wrong"));
        }

        return Result<AnalyticData>.Success(data);
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


    public Task<GpxFileDto> GetByIdAsync(Guid id) {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(Guid id) {
        throw new NotImplementedException();
    }
}


internal static class FileValidation {
    public static (bool isValid, List<Error> errors) Validate(IFormFile? file) {
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
