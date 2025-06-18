using Domain.Common.Result;

namespace Domain.Common;

public interface IRule {
    Result<bool> Check();
    string Message { get; }
}

public class ValidationRule(string name) : IRule {
    public string Message => "Name must not be empty";

    public Result<bool> Check() {
        if (name == null) {
            return Result<bool>.Failure(Errors.RuleViolation(this));
        }
        return Result<bool>.Success(true);
    }
}
