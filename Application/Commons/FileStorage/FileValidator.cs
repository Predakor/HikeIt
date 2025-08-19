using Domain.Common.Result;
using Domain.Common.Validations;
using Microsoft.AspNetCore.Http;
using System.Collections.Immutable;

namespace Application.Commons.FileStorage;

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
        AddRule(new FileRules.FileRules.MaxSize(_file, maxSize * 1024 * 1024));

    public FileValidator HasValidExtention(ImmutableArray<string> extentions) =>
        AddRule(new FileRules.FileRules.AllowedExtention(_file, extentions));

    public Result<IFormFile> Validate() {
        while (rules.TryDequeue(out IRule? rule)) {
            if (rule.Check().HasErrors(out var error)) {
                return error;
            }
        }

        return Result<IFormFile>.Success(_file);
    }
}
