namespace Domain.Common;

public static class Errors {
    public static Error Unknown(string message) => new Error.Unknown(message);
    public static Error DbError(string message) => new Error.DbError(message);
    public static Error NotFound(string message) => new Error.NotFound(message);
    public static Error BadRequest(string message) => new Error.BadRequest(message);
    public static Error Empty(string message) => new Error.EmptyCollection(message);
}

public abstract record Error(string Code, string Message) {
    internal sealed record NotFound(string Target)
        : Error("not_found", $"Entity '{Target}' was not found.");

    internal sealed record BadRequest(string Reason)
        : Error("bad_request", Reason);

    internal sealed record DbError(string Detail = "A database error occurred.")
        : Error("db_error", Detail);

    internal sealed record EmptyCollection(string Context = "Collection")
        : Error("empty", $"{Context} is empty.");

    internal sealed record Unknown(string Detail = "Something went wrong.")
        : Error("unknown", Detail);
}
