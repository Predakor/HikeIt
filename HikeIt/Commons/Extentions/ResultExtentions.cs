using Api.Commons.Factories;
using Microsoft.AspNetCore.Mvc;

namespace Api.Commons.Extentions;

public enum ResultType {
    ok,
    created,
    noContent,
}

internal static class ResultExtentions {
    public static IResult ToApiResult<T>(
        this Result<T> result,
        ResultType resultType = ResultType.ok
    ) {
        return (IResult)result.ToHttpResult(HttpResultFactory.MinimalApi, resultType);
    }

    public static async Task<IResult> ToApiResultAsync<T>(
        this Task<Result<T>> result,
        ResultType resultType = ResultType.ok
    ) {
        var res = await result;
        return res.ToApiResult(resultType);
    }

    public static IActionResult ToActionResult<T>(
        this Result<T> result,
        ResultType resultType = ResultType.ok
    ) {
        return (IActionResult)result.ToHttpResult(HttpResultFactory.Controller, resultType);
    }

    public static async Task<IActionResult> ToActionResultAsync<T>(
        this Task<Result<T>> result,
        ResultType resultType = ResultType.ok
    ) {
        var res = await result;
        return res.ToActionResult(resultType);
    }

    static object ToHttpResult<T>(
        this Result<T> result,
        IHttpResultFactory factory,
        ResultType resultType = ResultType.ok
    ) {
        return result.Match<T, object>(
            success => MapSucces(factory, resultType, success),
            error => MapError(factory, error)
        );
    }

    static object MapSucces<T>(IHttpResultFactory factory, ResultType resultType, T success) {
        return resultType switch {
            ResultType.ok => factory.Ok(success),
            ResultType.created => factory.Created(success?.ToString() ?? string.Empty),
            ResultType.noContent => factory.NoContent(),
            _ => throw new InvalidOperationException("Unhandled result type"),
        };
    }

    static object MapError(IHttpResultFactory factory, ResultError error) {
        var payload = new { error.Message, Code = error.Code.ToString() };
        return error.Code switch {
            ErrorCode.not_authorized => factory.Unauthorized(),
            ErrorCode.not_found => factory.NotFound(payload),
            ErrorCode.db_error => factory.BadRequest(payload),
            ErrorCode.empty_collection => factory.Ok(Array.Empty<object>()),
            _ => factory.BadRequest(payload),
        };
    }
}
