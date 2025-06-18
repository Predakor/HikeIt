namespace Domain.Common.Result;

public static class ResultExtentions {
    public static Result<TOut> Bind<TIn, TOut>(
        this Result<TIn> result,
        Func<TIn, Result<TOut>> func
    ) {
        return result.IsSuccess && result.Value is not null
            ? func(result.Value)
            : Result<TOut>.Failure(result.Error!);
    }

    // Sync Result<T> + async func
    public static async Task<Result<TOut>> Bind<TIn, TOut>(
        this Result<TIn> result,
        Func<TIn, Task<Result<TOut>>> func
    ) {
        return result.IsSuccess && result.Value is not null
            ? await func(result.Value)
            : Result<TOut>.Failure(result.Error!);
    }

    // Async Task<Result<T>> + sync func
    public static async Task<Result<TOut>> Bind<TIn, TOut>(
        this Task<Result<TIn>> resultTask,
        Func<TIn, Result<TOut>> func
    ) {
        var result = await resultTask;
        return result.IsSuccess && result.Value is not null
            ? func(result.Value)
            : Result<TOut>.Failure(result.Error!);
    }

    // Async Task<Result<T>> + async func
    public static async Task<Result<TOut>> Bind<TIn, TOut>(
        this Task<Result<TIn>> resultTask,
        Func<TIn, Task<Result<TOut>>> func
    ) {
        var result = await resultTask;
        return result.IsSuccess && result.Value is not null
            ? await func(result.Value)
            : Result<TOut>.Failure(result.Error!);
    }

    public static void Match<T>(this Result<T> result, Action<T> onSuccess, Action<Error> onFailure) {
        if (result.IsSuccess && result.Value is not null)
            onSuccess(result.Value);
        else
            onFailure(result.Error!);
    }

    public static async Task MatchAsync<T>(
        this Result<T> result,
        Func<T, Task> onSuccess,
        Func<Error, Task> onFailure
    ) {
        if (result.IsSuccess && result.Value is not null)
            await onSuccess(result.Value);
        else
            await onFailure(result.Error!);
    }

    public static async Task MatchAsync<T>(
        this Task<Result<T>> result,
        Action<T> onSuccess,
        Action<Error> onFailure
    ) {
        var r = await result;
        if (r.IsSuccess && r.Value is not null)
            onSuccess(r.Value);
        else
            onFailure(r.Error!);
    }

    public static TReturn Map<TIn, TReturn>(
        this Result<TIn> result,
        Func<TIn, TReturn> onSuccess,
        Func<Error, TReturn> onFailure
    ) {
        return result.IsSuccess && result.Value is not null
            ? onSuccess(result.Value)
            : onFailure(result.Error!);
    }

    public static async Task<TReturn> MapAsync<TIn, TReturn>(
        this Result<TIn> result,
        Func<TIn, Task<TReturn>> onSuccess,
        Func<Error, Task<TReturn>> onFailure
    ) {
        return result.IsSuccess && result.Value is not null
            ? await onSuccess(result.Value)
            : await onFailure(result.Error!);
    }

    public static async Task<TReturn> MapAsync<TIn, TReturn>(
        this Task<Result<TIn>> result,
        Func<TIn, Task<TReturn>> onSuccess,
        Func<Error, Task<TReturn>> onFailure
    ) {
        var r = await result;
        return r.IsSuccess && r.Value is not null
            ? await onSuccess(r.Value)
            : await onFailure(r.Error!);
    }
}
