using Domain.Users;

namespace Application.Dto;

public abstract record UserDto(string UserName) {
    public record WithEmail(string UserName, string Email) : UserDto(UserName);

    public record WithBirthDay(string UserName, DateOnly BirthDay) : UserDto(UserName);

    public record PublicProfile(string FirstName, string UserName, string Avatar = "")
        : UserDto(UserName);

    public record Complete(
        string FirstName,
        string LastName,
        string UserName,
        string Email,
        DateOnly BirthDay
    ) : UserDto(UserName);

    public record Basic(string UserName) : UserDto(UserName) {
        public string FirstName { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string Avatar { get; init; } = string.Empty;
        public string[] Roles { get; init; } = [];
    }

    public record Register(
        string FirstName,
        string LastName,
        string Email,
        string Password,
        string UserName
    ) : UserDto(UserName);

    public record Login(string UserName, string Password);
}

public static class UserDtoFactory {
    public static UserDto.Basic ToBasic(this User user, string[] roles) {
        return new(user.UserName!) {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email!,
            Avatar = user.Avatar ?? "",
            Roles = roles,
        };
    }

    public static UserDto.Complete ToComplete(this User user) {
        return new(
            user.FirstName,
            user.LastName,
            user.UserName ?? "",
            user.Email ?? "",
            user.BirthDay
        );
    }

    public static UserDto.PublicProfile ToPublicProfile(this User user) {
        return new(user.FirstName, user.UserName, user.Avatar = "");
    }
}
