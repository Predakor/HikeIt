namespace Domain.Common;

public static class Errors {
    public static Error Unknown(string? message = "") => new Error.Unknown(message);

    public static Error DbError(string message) => new Error.DbError(message);

    public static Error NotFound(string message) => new Error.NotFound(message);

    public static Error BadRequest(string message) => new Error.BadRequest(message);

    public static Error EmptyCollection(string message) => new Error.EmptyCollection(message);

    public static Error RuleViolation(IRule rule) => new Error.RuleViolation(rule);

    public static Error File(string message = "") => new Error.File(message);
    public static Error NotAuthorized() => new Error.NotAuthorized();
}

public abstract record Error(string Code, string Message) {
    public sealed record NotFound(string Target)
        : Error("not_found", $"Entity '{Target}' was not found.");

    internal sealed record BadRequest(string Reason) : Error("bad_request", Reason);

    internal sealed record DbError(string Detail = "A database error occurred.")
        : Error("db_error", Detail);

    internal sealed record EmptyCollection(string Context = "Collection")
        : Error("empty", $"{Context} is empty.");

    internal sealed record Unknown(string? Detail)
        : Error("unknown", Detail ?? "Something went wrong.");

    internal sealed record File(string? Detail)
        : Error("file", Detail ?? "something went wrong while proccesing your file.");

    internal sealed record RuleViolation(IRule Rule)
        : Error("rule_violation", $"{Rule}: {Rule.Message}" ?? "Rule Violation error.");

    internal sealed record NotAuthorized()
        : Error("not_authorized", $"you're not authorized to do this please log in");
}
