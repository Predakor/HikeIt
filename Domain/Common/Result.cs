using System.Diagnostics.CodeAnalysis;

namespace Domain.Common;

public class Result<TResult> {
    public TResult? Value { get; }
    public Error? Error { get; }
    public bool IsSuccess => Error is null;

    private Result(Error error) {
        Error = error;
        Value = default;
    }

    private Result(TResult value) {
        Value = value;
        Error = default;
    }

    public static Result<TResult> Success(TResult value) => new(value);

    public static Result<TResult> Failure(Error error) => new(error);

    public TReturn Map<TReturn>(Func<TResult, TReturn> onSuccess, Func<Error, TReturn> onFailure) =>
        this switch {
            { IsSuccess: true, Value: not null } => onSuccess(Value),
            { Error: not null } => onFailure(Error),
            _ => throw new InvalidOperationException("Invalid Result state."),
        };

    public void Match(Action<TResult> onSuccess, Action<Error> onFailure) {
        if (IsSuccess && Value is not null) {
            onSuccess(Value);
        }
        else if (Error is not null) {
            onFailure(Error);
        }
        else {
            throw new InvalidOperationException("Invalid Result state.");
        }
    }

    public bool HasErrors([NotNullWhen(true)] out Error? error) {
        if (Error != null) {
            error = Error!;
            return true;
        }
        error = null;
        return false;
    }
}

public record Error(string Code, string Message) {
    public static Error NotFound(string message) => new("not found", message);
    public static Error BadRequest(string message) => new("bad request", message);
    public static Error Unknown(string? message = "") => new("unknown", message ?? "");
}

