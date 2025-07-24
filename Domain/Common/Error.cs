namespace Domain.Common;

public static class Errors {
    public static Error Unknown(string? message = "") => new Error.Unknown(message);

    public static Error DbError(string message) => new Error.DbError(message);

    public static Error NotFound(string message) => new Error.NotFound(message);

    public static Error NotFound<T>(string resourceName, string filterName, T filterValue) =>
        new Error.NotFound($"{resourceName} with {filterName}: {filterValue}");

    public static Error BadRequest(string message) => new Error.BadRequest(message);

    public static Error EmptyCollection(string message) => new Error.EmptyCollection(message);

    public static Error RuleViolation(IRuleBase rule) => new Error.RuleViolation(rule);

    public static Error File(string message = "") => new Error.File(message);

    public static Error NotAuthorized() => new Error.NotAuthorized();

    public static Error InvalidCredentials() => new Error.InvalidCredentials();

    public static Error NotUnique(string ItemName) => new Error.NotUnique(ItemName);
}

public enum ErrorCode {
    not_found,
    bad_request,
    db_error,
    empty,
    unknown,
    file,
    rule_violation,
    not_authorized,
    invalid_credentials,
    not_unique,
    empty_collection,
}

public abstract record Error(ErrorCode Code, string Message) {
    public sealed record NotFound(string Target)
        : Error(ErrorCode.not_found, $"{Target} was not found.");

    public sealed record BadRequest(string Reason) : Error(ErrorCode.bad_request, Reason);

    public sealed record DbError(string Detail = "A database error occurred.")
        : Error(ErrorCode.db_error, Detail);

    public sealed record EmptyCollection(string Context = "Collection")
        : Error(ErrorCode.empty_collection, $"{Context} is empty.");

    public sealed record Unknown(string? Detail = null)
        : Error(ErrorCode.unknown, Detail ?? "Something went wrong.");

    public sealed record File(string? Detail = null)
        : Error(ErrorCode.file, Detail ?? "Something went wrong while processing your file.");

    public sealed record RuleViolation(IRuleBase Rule)
        : Error(ErrorCode.rule_violation, $"{Rule}: {Rule.Message}" ?? "Rule Violation error.");

    public sealed record NotAuthorized()
        : Error(ErrorCode.not_authorized, "You're not authorized to do this. Please log in.");

    public sealed record InvalidCredentials()
        : Error(ErrorCode.invalid_credentials, "Invalid login or password");

    public sealed record NotUnique(string ItemName)
        : Error(ErrorCode.not_unique, $"{ItemName} already exists");
}
