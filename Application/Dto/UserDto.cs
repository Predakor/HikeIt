namespace Application.Dto;

public abstract record UserDto(string UserName) {
    public record Basic(string UserName) : UserDto(UserName);

    public record WithEmail(string UserName, string Email) : UserDto(UserName);

    public record WithBirthDay(string UserName, DateOnly BirthDay) : UserDto(UserName);

    public record Complete(string UserName, string Email, DateOnly BirthDay) : UserDto(UserName);

    public record Register(
        string FirstName,
        string LastName,
        string Email,
        string Password,
        string UserName
    ) : UserDto(UserName);

    public record Login(
        string UserName,
        string Password
    );
}
