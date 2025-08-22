using Microsoft.AspNetCore.Mvc;

namespace Api.Commons.Factories;
public interface IHttpResultFactory {
    object Ok(object value);
    object Created(string location, object? value = null);
    object NoContent();
    object BadRequest(object? error);
    object NotFound(object? error);
    object Unauthorized();
}
internal static class HttpResultFactory {
    public static readonly IHttpResultFactory Controller = new MvcResult();
    public static readonly IHttpResultFactory MinimalApi = new MinimalApiResult();

    class MvcResult : IHttpResultFactory {
        public object Ok(object value) => new OkObjectResult(value);

        public object Created(string location, object? value = null) =>
            new CreatedResult(location, value);

        public object NoContent() => new NoContentResult();

        public object BadRequest(object? error) => new BadRequestObjectResult(error);

        public object NotFound(object? error) => new NotFoundObjectResult(error);

        public object Unauthorized() => new UnauthorizedResult();
    }

    class MinimalApiResult : IHttpResultFactory {
        public object Ok(object value) => Results.Ok(value);

        public object Created(string location, object? value = null) =>
            Results.Created(location, value);

        public object NoContent() => Results.NoContent();

        public object BadRequest(object? error) => Results.BadRequest(error);

        public object NotFound(object? error) => Results.NotFound(error);

        public object Unauthorized() => Results.Unauthorized();
    }
}
