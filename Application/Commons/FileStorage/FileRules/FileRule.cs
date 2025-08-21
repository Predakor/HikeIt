using Domain.Common;
using Domain.Common.Result;
using Domain.Common.Validations;
using Microsoft.AspNetCore.Http;
using System.Collections.Immutable;

namespace Application.Commons.FileStorage.FileRules;

internal abstract class FileRule {
    public sealed class MaxSize(IFormFile file, double maxSize) : IRule {
        public string Name => "File too large";
        public string Message => $"Max file size is: {maxSize / 1024f / 1024f:F1} MB.";

        public Result<bool> Check() {
            if (file.Length > maxSize) {
                return Errors.RuleViolation(this);
            }
            return true;
        }
    }

    public sealed class AllowedExtention(IFormFile file, ImmutableArray<string> allowedExtensions)
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
}