using Domain.Common;
using Domain.Common.Result;
using Domain.Common.Validations;
using Domain.Trips.Entities.GpxFiles;
using Domain.Trips.ValueObjects;
using Microsoft.AspNetCore.Http;

namespace Application.Services.Files;

public class GpxFileService(IGpxFileStorage storage, IGpxFileRepository repository, IGpxParser parser)
    : IGpxFileService {
    readonly IGpxFileStorage _fileStorage = storage;
    readonly IGpxFileRepository _repository = repository;
    readonly IGpxParser _parser = parser;

    public async Task<Result<GpxFile>> CreateAsync(IFormFile file, Guid userId, Guid tripId) {
        return await _fileStorage
            .Save(file, userId.ToString())
            .MapAsync(info => new GpxFile() {
                Id = tripId,
                Path = info.Path,
                Name = info.Name,
                OriginalName = file.Name,
                CreatedAt = DateTime.UtcNow,
            });
    }

    public async Task<Result<AnalyticData>> ExtractGpxData(IFormFile file) {
        try {
            return await _parser.ParseAsync(file.OpenReadStream());
        }
        catch (Exception ex) {
            return Errors.Unknown(ex.Message);
        }
    }

    public async Task<Result<AnalyticData>> ExtractGpxData(Guid id) {
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

    public Result<IFormFile> Validate(IFormFile file) {
        var (isValid, errors) = FileValidation.Validate(file);
        if (!isValid) {
            string errorstring = errors.Select(e => e.ToString()).ToString();
            return Errors.Unknown(errorstring);
        }
        return Result<IFormFile>.Success(file);
    }
}

internal static class FileValidation {
    class MaxFileSizeRule(IFormFile file, double maxSize) : IRule {
        public string Name => "File size";
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
        public string Name => "Invalid extention";
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
            if (rule.Check().HasErrors(out var error)) {
                errors.Add(error);
            }
        }

        return (errors.Count == 0, errors);
    }
}
