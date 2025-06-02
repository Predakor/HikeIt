using Application.Dto;
using Application.Services.Users;
using Domain.Entiites.Users;

namespace Api.Endpoints;

public static class UsersEndpoints {
    public static RouteGroupBuilder MapUserEndpoints(this WebApplication app) {
        var group = app.MapGroup("api/users");

        group.MapGet("/", GetAll);
        group.MapGet("/{id}", GetById);
        group.MapPost("/", CreateUser).Produces<User>();

        return group;
    }

    static async Task<IResult> GetAll(IUserService service) {
        var results = await service.GetAllUsersAsync();
        if (results is null) {
            return Results.NotFound();
        }
        return Results.Ok(results);
    }

    static async Task<IResult> GetById(IUserService service, Guid id) {
        var result = await service.GetUserByIdAsync(id);
        if (result is null) {
            return Results.NotFound();
        }
        return Results.Ok(result);
    }

    static async Task<IResult> CreateUser(IUserService service, UserDto.Complete userDto) {
        await service.CreateUserAsync(userDto);
        return Results.Ok();
    }
}
