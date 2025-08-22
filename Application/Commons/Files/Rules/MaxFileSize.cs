using Microsoft.AspNetCore.Http;

namespace Application.Commons.Files.Rules;

internal sealed class MaxFileSize(IFormFile file, double maxSize) : IRule {
    public string Name => "File too large";
    public string Message => $"Max file size is: {maxSize / 1024f / 1024f:F1} MB.";

    public Result<bool> Check() {
        if (file.Length > maxSize) {
            return Errors.RuleViolation(this);
        }
        return true;
    }
}
