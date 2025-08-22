using Core.Abstractions;

namespace Core.Validators.Rules;

internal class NotNullRule<T>(T value) : IRule {
    public string Name => "Invalid value";

    public string Message => "Value cannot be null";

    public Result<bool> Check() {
        if (value is null) {
            return Errors.RuleViolation(this);
        }

        return true;
    }
}
