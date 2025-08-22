using Application.Commons.Files.Rules;
using Domain.Common.Result;
using Domain.Common.Validations.Validators;
using Microsoft.AspNetCore.Http;
using System.Collections.Immutable;

namespace Application.Commons.Files;

public class FileValidator : AbstractValidator<IFormFile> {
    public FileValidator HasMaxSizeMB(double maxSize) {
        AddRule(f => new MaxFileSize(f, maxSize * 1024 * 1024));
        return this;
    }

    public FileValidator HasValidExtention(ImmutableArray<string> extentions) {
        AddRule(f => new AllowedFileExtentions(f, extentions));
        return this;
    }

    public FileValidator HasValidExtention(string extention) {
        AddRule(f => new AllowedFileExtentions(f, [extention]));
        return this;
    }

    static readonly FileValidator Gpx = new FileValidator()
        .HasMaxSizeMB(0.5)
        .HasValidExtention([".gpx"]);

    static readonly FileValidator Avatar = new FileValidator()
        .HasMaxSizeMB(0.5)
        .HasValidExtention([".png", ".jpg", ".webp", ".avif"]);

    public static Result<IFormFile> ValidateGpx(IFormFile file) => Gpx.Validate(file);

    public static Result<IFormFile> ValidateAvatar(IFormFile file) => Avatar.Validate(file);
}
