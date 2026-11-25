namespace Core.Results;

public static class ResultExtentions
{
    public static Result<TReturn> Map<TIn, TReturn>(this Result<TIn> result, Func<TIn, TReturn> map)
    {
        return result.IsSuccess
            ? Result<TReturn>.Success(map(result.Value!))
            : Result<TReturn>.Failure(result.Error!);
    }

    public static TResult Match<TIn, TResult>(
        this Result<TIn> result,
        Func<TIn, TResult> mapValue,
        Func<ResultError, TResult> mapError
    )
    {
        return result.IsSuccessWithValue()
            ? mapValue(result.Value!)
            : mapError(result.Error!);
    }

    public static Result<TOut> Bind<TIn, TOut>(this Result<TIn> result, Func<TIn, Result<TOut>> func)
    {
        return result.IsSuccessWithValue()
            ? func(result.Value!)
            : Result<TOut>.Failure(result.Error!);
    }

    public static Result<TIn> Tap<TIn>(this Result<TIn> result, Action<TIn> effect)
    {
        if (result.IsSuccessWithValue())
        {
            effect(result.Value!);
        }

        return result;
    }

    public static Result<IEnumerable<T>> EnsureNotEmpty<T>(this IEnumerable<T> source, string notFoundMessage)
    {
        return source.Any()
            ? Result<IEnumerable<T>>.Success(source)
            : Result<IEnumerable<T>>.Failure(Errors.NotFound(notFoundMessage));
    }

    public static Result<IList<T>> EnsureNotEmpty<T>(this IList<T> source, string notFoundMessage)
    {
        return source.Count is not 0
            ? Result<IList<T>>.Success(source)
            : Result<IList<T>>.Failure(Errors.NotFound(notFoundMessage));
    }

    private static bool IsSuccessWithValue<TIn>(this Result<TIn> result)
    {
        return result.IsSuccess && result.Value is not null;
    }
}
