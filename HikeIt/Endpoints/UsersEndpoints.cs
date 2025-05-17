using HikeIt.Api.Dto;
using HikeIt.Api.Entities;
using HikeIt.Api.Repository;

namespace HikeIt.Api.Endpoints;

public static class UsersEndpoints {
    public static RouteGroupBuilder MapUserEndpoints(this WebApplication app) {
        var group = app.MapGroup("api/users");

        group.MapGet("/", GetAll);
        group.MapGet("/{id}", GetById);
        group.MapPost("/", CreateUser).Produces<User>();

        return group;
    }

    static async Task<IResult> GetAll(IRepository<User> repo) {
        var results = await repo.GetAllAsync();
        if (results is null) {
            return Results.NotFound();
        }
        return Results.Ok(results);
    }
    static async Task<IResult> GetById(IRepository<User> repo, int id) {
        var result = await repo.GetByIDAsync(id);
        if (result is null) {
            return Results.NotFound();
        }
        return Results.Ok(result);
    }

    static async Task<IResult> CreateUser(IRepository<User> repo, UserDto.Complete userDto) {
        User newUser = new() {
            Name = userDto.Name,
            Email = userDto.Email,
            BirthDay = userDto.BirthDay,
        };

        await repo.AddAsync(newUser);
        return Results.Ok();
    }

}
