using Domain.Common.Result;

namespace Domain.Common.Validations;

public interface IRuleBase {
    string Name { get; }
    string Message { get; }
}

public interface IRule : IRuleBase {
    Result<bool> Check();
}

public interface IRuleAsync : IRuleBase {
    Task<Result<bool>> CheckAsync();
}

public static class RuleChecker {
    public static Result<bool> Check(IRule rule) => rule.Check();
    public static async Task<Result<bool>> CheckAsync(IRuleAsync rule) => await rule.CheckAsync();
}

public class ValidationRule(string name) : IRule {
    public string Message => "Name must not be empty";

    public string Name => "validation";

    public Result<bool> Check() {
        if (name == null) {
            return Result<bool>.Failure(Errors.RuleViolation(this));
        }
        return Result<bool>.Success(true);
    }
}
