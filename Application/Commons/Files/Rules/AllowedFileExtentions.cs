using Domain.Common;
using Domain.Common.Result;
using Domain.Common.Validations;
using Microsoft.AspNetCore.Http;
using System.Collections.Immutable;

namespace Application.Commons.Files.Rules;

internal sealed class AllowedFileExtentions(IFormFile file, ImmutableArray<string> allowedExtensions)
    : IRule {
    public string Name => "Invalid extention";
    public string Message => $"Only {string.Join(", ", allowedExtensions)} files are allowed.";

    public Result<bool> Check() {
        foreach (string extension in allowedExtensions) {
            if (file.FileName.EndsWith(extension, StringComparison.OrdinalIgnoreCase)) {
                return true;
            }
        }

        return Errors.RuleViolation(this);
    }
}
