using Microsoft.AspNetCore.Mvc;
using System.Collections;

namespace HikeIt.Api.ApiResolver;
public class MinimalAPiResolver : IRequestResolver<IResult> {
    public IResult Resolve<T>(T? result) {
        if (result is null) {
            return Results.NotFound();
        }

        if (result is IEnumerable collection and not string) {
            var enumerator = collection.GetEnumerator();
            if (!enumerator.MoveNext()) {
                return Results.NotFound();
            }
        }

        return Results.Ok(result);
    }
}

public class ControllerApiResolver : IRequestResolver<IActionResult> {
    public IActionResult Resolve<T>(T? result) {
        if (result is null) {
            return new NotFoundResult();
        }

        if (result is IEnumerable collection and not string) {
            var enumerator = collection.GetEnumerator();
            if (!enumerator.MoveNext()) {
                return new NotFoundResult();
            }
        }

        return new OkObjectResult(result);
    }
}