namespace Core.Results;

public static class ResultExtentions {
    public static Result<TReturn> Map<TIn, TReturn>(this Result<TIn> result, Func<TIn, TReturn> map) {
        return result.IsSuccess
            ? Result<TReturn>.Success(map(result.Value!))
            : Result<TReturn>.Failure(result.Error!);
    }

    public static TResult Match<TIn, TResult>(
        this Result<TIn> result,
        Func<TIn, TResult> mapValue,
        Func<ResultError, TResult> mapError
    ) {
        return result.IsSuccessWithValue() ? mapValue(result.Value!) : mapError(result.Error!);
    }

    public static Result<TOut> Bind<TIn, TOut>(
        this Result<TIn> result,
        Func<TIn, Result<TOut>> func
    ) {
        return result.IsSuccessWithValue()
            ? func(result.Value!)
            : Result<TOut>.Failure(result.Error!);
    }

    public static Result<TIn> Tap<TIn>(this Result<TIn> result, Action<TIn> effect) {
        if (result.IsSuccessWithValue()) {
            effect(result.Value!);
        }

        return result;
    }

    static bool IsSuccessWithValue<TIn>(this Result<TIn> result) {
        return result.IsSuccess && result.Value is not null;
    }
}
