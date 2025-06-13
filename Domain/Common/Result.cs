using System.Diagnostics.CodeAnalysis;

namespace Domain.Common;

public class Result<TResult> {
    public TResult? Value { get; protected set; }
    public Error? Error { get; protected set; }
    public bool IsSuccess => Error is null;

    protected Result(Error error) {
        Error = error;
        Value = default;
    }

    protected Result(TResult value) {
        Value = value;
        Error = default;
    }

    public static Result<TResult> Success(TResult value) => new(value);

    public static Result<TResult> Failure(Error error) => new(error);

    public TReturn Map<TReturn>(Func<TResult, TReturn> onSuccess, Func<Error, TReturn> onFailure) {
        return this switch {
            { IsSuccess: true, Value: not null } => onSuccess(Value),
            { Error: not null } => onFailure(Error),
            _ => throw new InvalidOperationException("Invalid Result state."),
        };
    }

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

    public TReturn Map<TReturn>(
        Func<TResult, TReturn> onSuccess,
        Func<Error, TReturn> onNotFound,
        Func<Error, TReturn> onFailure
    ) {
        return this switch {
            { IsSuccess: true, Value: not null } => onSuccess(Value),
            { Error.Code: "not found" } => onNotFound(Error),
            { Error: not null } => onFailure(Error),
            _ => throw new InvalidOperationException("Invalid Result state."),
        };
    }

    public async Task<TReturn> AsyncMap<TReturn>(
        Func<TResult, Task<TReturn>> onSuccess,
        Func<Error, Task<TReturn>> onNotFound,
        Func<Error, Task<TReturn>> onFailure
    ) {
        return this switch {
            { IsSuccess: true, Value: not null } => await onSuccess(Value),
            { Error.Code: "not found" } => await onNotFound(Error),
            { Error: not null } => await onFailure(Error),
            _ => throw new InvalidOperationException("Invalid Result state."),
        };
    }

    public void Match(Action<TResult> onSuccess, Action<Error> onNotFound, Action<Error> onFailure) {
        if (IsSuccess && Value is not null) {
            onSuccess(Value);
        }
        else if (Error is not null) {
            if (Error.Code == "not found") {
                onNotFound(Error);
            }
            else {
                onFailure(Error);
            }
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
