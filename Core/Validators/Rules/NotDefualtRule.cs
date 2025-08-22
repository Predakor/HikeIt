using Core.Abstractions;

namespace Core.Validators.Rules;

internal class NotDefualtRule<T>(T value) : IRule {
    public string Name => "Invalid value";

    public string Message => "Value cannot be null";

    public Result<bool> Check() {
        if (EqualityComparer<T>.Default.Equals(value, default!)) {
            return Errors.RuleViolation(this);
        }

        return true;
    }
}
