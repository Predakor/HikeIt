using Domain.Common.Result;

namespace Domain.Common.Validations.Validators;

public class AbstractValidator<T> {
    readonly LinkedList<Func<T, IRule>> _rules = new();

    public AbstractValidator<T> AddRule(Func<T, IRule> ruleFactory) {
        _rules.AddLast(ruleFactory);
        return this;
    }

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
