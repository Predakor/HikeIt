
using System.Diagnostics.CodeAnalysis;

namespace Domain.Common.Result;

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

    public static implicit operator Result<TResult>(TResult value) => new(value);

    public static implicit operator Result<TResult>(Error Error) => new(Error);


    [Obsolete]
    public TReturn Map<TReturn>(Func<TResult, TReturn> onSuccess, Func<Error, TReturn> onFailure) {
        return this switch {
            { IsSuccess: true, Value: not null } => onSuccess(Value),
            { Error: not null } => onFailure(Error),
            _ => throw new InvalidOperationException("Invalid Result state."),
        };
    }

    [Obsolete]
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

    [Obsolete]
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

    public bool HasErrors([NotNullWhen(true)] out Error? error) {
        if (Error != null) {
            error = Error!;
            return true;
        }
        error = null;
        return false;
    }
}

