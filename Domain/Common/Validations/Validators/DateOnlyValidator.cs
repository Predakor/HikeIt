using Domain.Common.Result;

namespace Domain.Common.Validations.Validators;

public class DateOnlyValidator : AbstractValidator<DateOnly> {
    public DateOnlyValidator()
        : base() { }

    public DateOnlyValidator NotInTheFuture() {
        AddRule(t => new DateOnlyRules.DateNotInTheFuture(t));
        return this;
    }

    public DateOnlyValidator MinimumDate(DateOnly minTime) {
        AddRule(t => new DateOnlyRules.DateNotSmallerThan(t, minTime));
        return this;
    }

    public static class BirthdayValidator {
        static readonly DateOnlyValidator _instance = new DateOnlyValidator()
            .NotInTheFuture()
            .MinimumDate(new DateOnly(1900, 1, 1));

        public static Result<DateOnly> ValidateBirthday(DateOnly date) => _instance.Validate(date);
    }
}

internal abstract class DateOnlyRules {
    public sealed class DateNotInTheFuture(DateOnly time) : IRule {
        public string Name => "Invalid Time";
        public string Message => "Date is set in the future";

        public Result<bool> Check() {
            if (time > DateOnly.FromDateTime(DateTime.UtcNow))
                return Errors.RuleViolation(this);

            return true;
        }
    }

    public sealed class DateNotSmallerThan(DateOnly time, DateOnly minTime) : IRule {
        public string Name => "Invalid Time";
        public string Message => $"Date must be greater than {minTime}";

        public Result<bool> Check() {
            if (time < minTime)
                return Errors.RuleViolation(this);

            return true;
        }
    }
}
