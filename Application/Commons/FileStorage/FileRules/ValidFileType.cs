using Domain.Common;
using Domain.Common.Result;
using Microsoft.AspNetCore.Http;
using System.Collections.Immutable;

namespace Application.Commons.FileStorage.FileRules;

internal abstract class FileRules {
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

public class FileValidator {
    readonly IFormFile _file;
    readonly Queue<IRule> rules = [];

    public FileValidator(IFormFile file) {
        _file = file;
    }

    FileValidator AddRule(IRule rule) {
        rules.Enqueue(rule);
        return this;
    }

    public FileValidator HasMaxSizeMB(double maxSize) =>
        AddRule(new FileRules.MaxSize(_file, maxSize * 1024 * 1024));

    public FileValidator HasValidExtention(ImmutableArray<string> extentions) =>
        AddRule(new FileRules.AllowedExtention(_file, extentions));

    public Result<IFormFile> Validate() {
        while (rules.TryDequeue(out IRule? rule)) {
            if (rule.Check().HasErrors(out var error)) {
                return error;
            }
        }

        return Result<IFormFile>.Success(_file);
    }
}
