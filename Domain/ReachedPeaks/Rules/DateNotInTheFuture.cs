using Domain.Common;
using Domain.Common.Result;

namespace Domain.ReachedPeaks.Rules;

public class DateNotInTheFuture(DateTime time) : IRule {
    public string Name => "Invalid Time";
    public string Message => "Time is set to a future date please enter correct date";

    public Result<bool> Check() {
        if (time <= DateTime.UtcNow) {
            return true;
        }
        return Errors.RuleViolation(this);
    }
}
