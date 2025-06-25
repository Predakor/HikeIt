using Domain.Common;
using Domain.Common.Result;
using Microsoft.AspNetCore.Mvc;

namespace Api.Extentions;

public static class ResultExtentios {
    internal static IActionResult ToActionResult<T>(this Result<T> result) {
        return result.Match<T, IActionResult>(
            data => new OkObjectResult(data),
            error =>
                error switch {
                    Error.NotAuthorized => new UnauthorizedResult(),
                    Error.NotFound => new NotFoundObjectResult(error.Message),
                    Error.DbError => new BadRequestObjectResult(error),
                    _ => new BadRequestObjectResult(error),
                }
        );
    }

    internal static async Task<IActionResult> ToActionResultAsync<T>(this Task<Result<T>> result) {
        return (await result).ToActionResult();
    }
}
