using Domain.Common.Result;

namespace Domain.Common.Validations.Validators;

public class AbstractValidator<T> {
    readonly LinkedList<Func<T, IRule>> _rules = new();

    public AbstractValidator<T> AddRule(Func<T, IRule> ruleFactory) {
        _rules.AddLast(ruleFactory);
        return this;
    }

    public AbstractValidator<T> NotNull() => AddRule(t => new NotNullRule<T>(t));

    public AbstractValidator<T> NotDefault() => AddRule(t => new NotDefualtRule<T>(t));

    public Result<T> Validate(T target) {
        foreach (var makeRule in _rules) {
            var rule = makeRule(target);
            if (rule.Check().HasErrors(out var error)) {
                return error;
            }
        }
        return target;
    }
}

class NotNullRule<T>(T value) : IRule {
    public string Name => "Invalid value";

    public string Message => "Value cannot be null";

    public Result<bool> Check() {
        if (value is null) {
            return Errors.RuleViolation(this);
        }

        return true;
    }
}

class NotDefualtRule<T>(T value) : IRule {
    public string Name => "Invalid value";

    public string Message => "Value cannot be null";

    public Result<bool> Check() {
        if (EqualityComparer<T>.Default.Equals(value, default!)) {
            return Errors.RuleViolation(this);
        }

        return true;
    }
}
