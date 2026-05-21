using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Users;

[Authorize]
[ApiController]
public abstract class UserControllerBase : ControllerBase
{
    internal const string RouteBase = "api/users/me";
}
