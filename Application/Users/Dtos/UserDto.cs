using Domain.Users;
using static Application.Users.Dtos.UserDataDto;

namespace Application.Users.Dtos;

public abstract record UserDto(string UserName) {
    public sealed record Basic(string UserName, string[] Roles, string Avatar);

    public sealed record Profile(
        PublicProfile Summary,
        Personal Personal,
        AccountState AccountState
    );

    public sealed record Complete(
        string FirstName,
        string LastName,
        string UserName,
        string Email,
        DateOnly BirthDay
    ) : UserDto(UserName);

    public sealed record Register(
        string FirstName,
        string LastName,
        string Email,
        string Password,
        string UserName
    ) : UserDto(UserName);

    public sealed record Login(string UserName, string Password);
}

public static class UsersDtoExtentions {
    public static PublicProfile ToPublicProfile(this User user) {
        return new() {
            UserName = user.UserName!,
            Avatar = user.Avatar,
            Rank = user?.Rank?.Name ?? "Novice Hiker",
            Peaks = user?.Stats.TotalPeaks ?? 0,
            Trips = user?.Stats.TotalTrips ?? 0,
            Traveled = user?.Stats.TotalDistanceMeters ?? 0,
        };
    }

    public static Personal ToPersonal(this User user) {
        return new() {
            BirthDay = user.BirthDay,
            Country = user.Country ?? "",
            Email = user.Email ?? "",
            FirstName = user.FirstName,
            LastName = user.LastName,
            Gender = user.Gender,
        };
    }

    public static AccountState ToAccountState(this User user) {
        return new() {
            CreatedAt = user.CreatedAt,
            Role = "user",
            Status = "active",
        };
    }

    public static UserDto.Basic ToBasic(this User user, string[] roles) {
        return new(UserName: user.UserName!, Roles: roles, Avatar: user.Avatar);
    }
}

public static class UserDtoFactory {
    public static UserDto.Complete ToComplete(this User user) {
        return new(
            user.FirstName,
            user.LastName,
            user.UserName ?? "",
            user.Email ?? "",
            user.BirthDay
        );
    }
}
