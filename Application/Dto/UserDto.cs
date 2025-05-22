namespace Application.Dto;

public abstract record UserDto(string Name) {
    public record Basic(string Name) : UserDto(Name);
    public record WithEmail(string Name, string Email) : UserDto(Name);
    public record WithBirthDay(string Name, DateOnly BirthDay) : UserDto(Name);
    public record Complete(string Name, string Email, DateOnly BirthDay) : UserDto(Name);
}
