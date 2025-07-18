using Domain.Common.Result;

namespace Domain.Common.Rules;
public class NotNullOrDefault<T>(T data, string? name = null) : IRule {
    public string Name => "not_null_or_default";

    public string Message => $"{name ?? "Value"} must not be null or default.";

    public Result<bool> Check() {

        if (data is null) {
            return Errors.RuleViolation(this);

        }

        if (EqualityComparer<T>.Default.Equals(data, default)) {
            return Errors.RuleViolation(this);
        }

        return true;
    }
}
