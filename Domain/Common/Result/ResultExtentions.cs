namespace Domain.Common.Result;
public static class ResultExtentions {
    public static Result<TReturn> Map<TIn, TReturn>(this Result<TIn> result, Func<TIn, TReturn> map) {
        return result.IsSuccess
            ? Result<TReturn>.Success(map(result.Value!))
            : Result<TReturn>.Failure(result.Error!);
    }

    public static TResult Match<TIn, TResult>(
        this Result<TIn> result,
        Func<TIn, TResult> mapValue,
        Func<Error, TResult> mapError
    ) {
        return result.IsSuccess && result.Value is not null
            ? mapValue(result.Value!)
            : mapError(result.Error!);
    }

    public static Result<TOut> Bind<TIn, TOut>(
        this Result<TIn> result,
        Func<TIn, Result<TOut>> func
    ) {
        return result.IsSuccess && result.Value is not null
            ? func(result.Value)
            : Result<TOut>.Failure(result.Error!);
    }


    //[Obsolete]
    //public static void Match<T>(this Result<T> result, Action<T> onSuccess, Action<Error> onFailure) {
    //    if (result.IsSuccess && result.Value is not null)
    //        onSuccess(result.Value);
    //    else
    //        onFailure(result.Error!);
    //}
}
