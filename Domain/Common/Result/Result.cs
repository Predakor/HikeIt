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

    public bool HasErrors([NotNull] out Error error) {
        if (Error != null) {
            error = Error!;
            return true;
        }
        error = null!;
        return false;
    }

    public TReturn Match<TReturn>(Func<TResult, TReturn> onSuccess, Func<Error, TReturn> onFailure) {
        return (IsSuccess && Value is not null) ? onSuccess(Value) : onFailure(Error!);
    }
}
