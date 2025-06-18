using Domain.Common;
using Domain.Common.Result;
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
            string errorstring = errors.Select(e => e.ToString()).ToString();
            var err = Errors.Unknown(errorstring);
            return err;
        }

        User? user = null;
        Guid userId = Guid.Parse("7a4f8c5b-19b7-4a6a-89c0-f9a2e98a9380");

        var result = await _fileStorage.Save(file, userId.ToString());

        if (result.HasErrors(out var error)) {
            Console.WriteLine(error.Message);
            return Errors.File(error.Message);
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

        return entity;
    }

    public async Task<AnalyticData> GetGpxDataFromFile(IFormFile file) {
        return await _parser.ParseAsync(file.OpenReadStream());
    }

    public async Task<Result<AnalyticData>> GetGpxDataByFileIdAsync(Guid id) {
        var result = await _repository.GetGpxFileStream(id);
        if (result == null) {
            return Errors.NotFound("No file with id found");
        }

        var data = await _parser.ParseAsync(result);
        if (data == null) {
            return Errors.Unknown("something went wrong");
        }

        return data;
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
    class MaxFileSizeRule(IFormFile file, double maxSize) : IRule {
        public string Message => $"File is too large. Max size: {maxSize:F1} MB.";

        public Result<bool> Check() {
            if (file.Length > maxSize) {
                double maxSizeInMB = maxSize / 1024f / 1024f;
                return Result<bool>.Failure(Errors.RuleViolation(this));
            }
            return Result<bool>.Success(true);
        }
    }

    class FileShouldBeOfType(IFormFile file, string allowedExtension) : IRule {
        public string Message =>
            $"\"Invalid extention\", $\"Only {{allowedExtension}} files are allowed.\"";

        public Result<bool> Check() {
            if (!file.FileName.EndsWith(allowedExtension, StringComparison.OrdinalIgnoreCase)) {
                return Result<bool>.Failure(Errors.RuleViolation(this));
            }

            return Result<bool>.Success(true);
        }
    }

    public static (bool isValid, List<Error> errors) Validate(IFormFile? file) {
        // Return immediately here; can't proceed with other checks
        if (file == null || file.Length == 0) {
            return (false, [Errors.NotFound("No file uploaded.")]);
        }

        var errors = new List<Error>();
        const int maxSizeInBytes = 1024 * 1024 / 2; // 0.5 MB
        const string allowedExtension = ".gpx";

        List<IRule> rules =
        [
            new MaxFileSizeRule(file, maxSizeInBytes),
            new FileShouldBeOfType(file, allowedExtension),
        ];

        foreach (var rule in rules) {
            rule.Check().Match(ok => { }, errors.Add);
        }

        return (errors.Count == 0, errors);
    }
}
