namespace Domain.Common.Result;

public static class AsyncResultExtentios {
    #region Bind

    //async result
    public static async Task<Result<T2>> BindAsync<T1, T2>(
        this Task<Result<T1>> result,
        Func<T1, Result<T2>> bind
    ) {
        return (await result).Bind(bind);
    }

    //async func
    public static async Task<Result<T2>> BindAsync<T1, T2>(
        this Result<T1> result,
        Func<T1, Task<Result<T2>>> bindAsync
    ) {
        return result.IsSuccess
            ? await bindAsync(result.Value!)
            : Result<T2>.Failure(result.Error!);
    }

    //async both
    public static async Task<Result<T2>> BindAsync<T1, T2>(
        this Task<Result<T1>> result,
        Func<T1, Task<Result<T2>>> bindAsync
    ) {
        return await (await result).BindAsync(bindAsync);
    }

    #endregion


    #region match
    public static async Task<TResult> MatchAsync<T, TResult>(
        this Task<Result<T>> result,
        Func<T, TResult> onSuccess,
        Func<Error, TResult> onFailure
    ) {
        var awaitedResult = await result;
        return awaitedResult.IsSuccess
            ? onSuccess(awaitedResult.Value!)
            : onFailure(awaitedResult.Error!);
    }

    public static async Task<TResult> MatchAsync<T, TResult>(
        this Result<T> result,
        Func<T, Task<TResult>> onSuccessAsync,
        Func<Error, TResult> onFailure
    ) {
        return result.IsSuccess && result.Value is not null
            ? await onSuccessAsync(result.Value!)
            : onFailure(result.Error!);
    }

    public static async Task<TResult> MatchAsync<T, TResult>(
        this Result<T> result,
        Func<T, TResult> onSuccess,
        Func<Error, Task<TResult>> onFailureAsync
    ) {
        return result.IsSuccess && result.Value is not null
            ? onSuccess(result.Value!)
            : await onFailureAsync(result.Error!);
    }

    //2/3 are async

    public static async Task<TResult> MatchAsync<T, TResult>(
        this Task<Result<T>> result,
        Func<T, Task<TResult>> onSucces,
        Func<Error, TResult> onFailureAsync
    ) {
        return await (await result).MatchAsync(onSucces, onFailureAsync);
    }

    public static async Task<TResult> MatchAsync<T, TResult>(
        this Task<Result<T>> result,
        Func<T, TResult> onSucces,
        Func<Error, Task<TResult>> onFailureAsync
    ) {
        return await (await result).MatchAsync(onSucces, onFailureAsync);
    }

    public static async Task<TResult> MatchAsync<T, TResult>(
        this Result<T> result,
        Func<T, Task<TResult>> onSuccessAsync,
        Func<Error, Task<TResult>> onFailureAsync
    ) {
        return result.IsSuccess
            ? await onSuccessAsync(result.Value!)
            : await onFailureAsync(result.Error!);
    }

    //all async
    public static async Task<TResult> MatchAsync<T, TResult>(
        this Task<Result<T>> result,
        Func<T, Task<TResult>> onSuccessAsync,
        Func<Error, Task<TResult>> onFailureAsync
    ) {
        return await (await result).MatchAsync(onSuccessAsync, onFailureAsync);
    }

    #endregion


    public static async Task<Result<TOut>> MapAsync<TIn, TOut>(
        this Task<Result<TIn>> result,
        Func<TIn, TOut> map
    ) {
        return (await result).Map(map);
    }

    public static async Task<Result<T2>> MapAsync<T1, T2>(
        this Result<T1> result,
        Func<T1, Task<T2>> mapAsync
    ) {
        return result.IsSuccess
            ? Result<T2>.Success(await mapAsync(result.Value!))
            : Result<T2>.Failure(result.Error!);
    }

    public static async Task<Result<T2>> MapAsync<T1, T2>(
        this Task<Result<T1>> result,
        Func<T1, Task<T2>> mapAsync
    ) {
        return await (await result).MapAsync(mapAsync);
    }
}
