using Domain.Entiites.Users;

namespace Application.Dto;

public abstract record UserDto(string UserName) {
    public record WithEmail(string UserName, string Email) : UserDto(UserName);

    public record WithBirthDay(string UserName, DateOnly BirthDay) : UserDto(UserName);

    public record Complete(string UserName, string Email, DateOnly BirthDay) : UserDto(UserName);

    public record Basic(
        string UserName,
        string FirstName,
        string LastName,
        string Email,
        string Avatar = ""
    ) : UserDto(UserName);

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
    public static UserDto.Basic CreateBasic(User user) {
        return new(user.UserName ?? "not specified", user.FirstName, user.LastName, user.Email ?? "unknown");
    }
}
