using Domain.Common;
using Domain.Common.Result;
using Microsoft.AspNetCore.Mvc;

namespace Api.Extentions;

public enum ResultType {
    ok,
    created,
    noContent,
}

public static class ResultExtentios {
    internal static IActionResult ToActionResult<T>(
        this Result<T> result,
        ResultType? resultType = ResultType.ok
    ) {
        return result.Match<T, IActionResult>(
            succes => MapSucces(resultType, succes),
            error => MapError(error)
        );
    }

    internal static async Task<IActionResult> ToActionResultAsync<T>(
        this Task<Result<T>> result,
        ResultType? resultType = ResultType.ok
    ) {
        return (await result).ToActionResult(resultType);
    }

    static IActionResult MapSucces<T>(ResultType? succesResponse, T data) {
        return succesResponse switch {
            ResultType.ok => new OkObjectResult(data),
            ResultType.created => new CreatedResult(data as string, null),
            ResultType.noContent => new NoContentResult(),
            _ => throw new Exception("unhandled respone type at Result Extentions "),
        };
    }

    record ResponseError(string Message, string Description);
    static ResponseError ToResponseError(this Error error) {
        return new ResponseError(error.Message, error.Code.ToString());
    }
    static IActionResult MapError(Error error) {
        return error.Code switch {
            ErrorCode.not_authorized => new UnauthorizedResult(),
            ErrorCode.not_found => new NotFoundObjectResult(error.ToResponseError()),
            ErrorCode.db_error => new BadRequestObjectResult(error.ToResponseError()),
            _ => new BadRequestObjectResult(error.ToResponseError()),
        };
    }
}
