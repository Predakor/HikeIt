namespace Domain.Common.Validations.Validators;

public class DateValidator : AbstractValidator<DateTime> {
    public DateValidator()
        : base() { }

    public DateValidator NotInTheFuture() {
        AddRule(t => new DateNotInTheFuture(t));
        return this;
    }

    public DateValidator MinimumDate(DateTime? minTime) {
        AddRule(t => new DateNotSmallerThan(
            t,
            minTime ?? new DateTime(1900, 1, 1).ToUniversalTime()
        ));
        return this;
    }

    public DateValidator InUtcFormat() {
        AddRule(t => new DateMustBeInUtc(t));
        return this;
    }

    public static class BirthdayValidator {
        static readonly DateValidator _instance = new DateValidator()
            .NotInTheFuture()
            .MinimumDate(new DateTime(1900, 1, 1))
            .InUtcFormat();

        public static Result<DateTime> ValidateBirthday(DateTime date) => _instance.Validate(date);
    }
}

internal sealed class DateNotInTheFuture(DateTime time) : IRule {
    public string Name => "Invalid Time";
    public string Message => "Time is set to a future date please enter correct date";

    public Result<bool> Check() {
        if (time > DateTime.UtcNow) {
            return Errors.RuleViolation(this);
        }

        return true;
    }
}

internal sealed class DateNotSmallerThan(DateTime time, DateTime minTime) : IRule {
    public string Name => "Invalid Time";
    public string Message => "Time must be greater than" + minTime.ToString();

    public Result<bool> Check() {
        if (minTime > time) {
            return Errors.RuleViolation(this);
        }

        return true;
    }
}

internal sealed class DateMustBeInUtc(DateTime time) : IRule {
    public string Name => "Invalid Date Format";
    public string Message => "Date must be in utc format";

    public Result<bool> Check() {
        if (time.Kind != DateTimeKind.Utc) {
            return Errors.RuleViolation(this);
        }

        return true;
    }
}
