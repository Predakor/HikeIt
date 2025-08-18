using Domain.Users;

namespace Application.Dto;

public abstract record UserDto(string UserName) {
    public sealed record Basic(string UserName, string[] Roles, string Avatar);

    public sealed record Personal {
        public required string FirstName { get; init; }
        public required string LastName { get; init; }
        public required string Email { get; init; }
        public required DateOnly BirthDay { get; init; }
        public required string Country { get; init; }
        public required Gender? Gender { get; init; }
    }

    public sealed record AccountState {
        public required string Role { get; init; }
        public required DateOnly CreatedAt { get; init; }
        public required string Status { get; init; } = "Active";
    }

    public sealed record PublicProfile() {
        public required string UserName { get; init; }
        public required string Avatar { get; init; }
        public required string Rank { get; init; }
        public required uint Trips { get; init; }
        public required uint Peaks { get; init; }
        public required uint Traveled { get; init; }
    }

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
    public static UserDto.PublicProfile ToPublicProfile(this User user) {
        return new() {
            UserName = user.UserName!,
            Avatar = user.Avatar,
            Rank = user?.Rank?.Name ?? "Novice Hiker",
            Peaks = user?.Stats.TotalPeaks ?? 0,
            Trips = user?.Stats.TotalTrips ?? 0,
            Traveled = user?.Stats.TotalDistanceMeters ?? 0,
        };
    }

    public static UserDto.Personal ToPersonal(this User user) {
        return new UserDto.Personal() {
            BirthDay = user.BirthDay,
            Country = user.Country ?? "",
            Email = user.Email ?? "",
            FirstName = user.FirstName,
            LastName = user.LastName,
            Gender = user.Gender,
        };
    }

    public static UserDto.AccountState ToAccountState(this User user) {
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
