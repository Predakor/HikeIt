using Core.Abstractions;

namespace Core.Results;

public static class Errors {
    public static ResultError Unknown(string? message = "") => new ResultError.Unknown(message);

    public static ResultError DbError(string message) => new ResultError.DbError(message);

    public static ResultError NotFound(string message) => new ResultError.NotFound(message);

    public static ResultError NotFound<T>(string resourceName, string filterName, T filterValue) =>
        new ResultError.NotFound($"{resourceName} with {filterName}: {filterValue}");

    public static ResultError NotFound<T>(string resourceName, T filterValue, string filterName = "id") =>
        new ResultError.NotFound($"{resourceName} with {filterName}: {filterValue}");

    public static ResultError BadRequest(string message) => new ResultError.BadRequest(message);

    public static ResultError EmptyCollection(string message) => new ResultError.EmptyCollection(message);

    public static ResultError RuleViolation(IRuleBase rule) => new ResultError.RuleViolation(rule);

    public static ResultError File(string message = "") => new ResultError.File(message);

    public static ResultError NotAuthorized() => new ResultError.NotAuthorized();

    public static ResultError InvalidCredentials() => new ResultError.InvalidCredentials();

    public static ResultError NotUnique(string ItemName) => new ResultError.NotUnique(ItemName);
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

public abstract record ResultError(ErrorCode Code, string Message) {
    public sealed record NotFound(string Target)
        : ResultError(ErrorCode.not_found, $"{Target} was not found.");

    public sealed record BadRequest(string Reason) : ResultError(ErrorCode.bad_request, Reason);

    public sealed record DbError(string Detail = "A database error occurred.")
        : ResultError(ErrorCode.db_error, Detail);

    public sealed record EmptyCollection(string Context = "Collection")
        : ResultError(ErrorCode.empty_collection, $"{Context} is empty.");

    public sealed record Unknown(string? Detail = null)
        : ResultError(ErrorCode.unknown, Detail ?? "Something went wrong.");

    public sealed record File(string? Detail = null)
        : ResultError(ErrorCode.file, Detail ?? "Something went wrong while processing your file.");

    public sealed record RuleViolation(IRuleBase Rule)
        : ResultError(ErrorCode.rule_violation, $"{Rule.Name}: {Rule.Message}" ?? "Rule Violation error.");

    public sealed record NotAuthorized()
        : ResultError(ErrorCode.not_authorized, "You're not authorized to do this. Please log in.");

    public sealed record InvalidCredentials()
        : ResultError(ErrorCode.invalid_credentials, "Invalid login or password");

    public sealed record NotUnique(string ItemName)
        : ResultError(ErrorCode.not_unique, $"{ItemName} already exists");
}
