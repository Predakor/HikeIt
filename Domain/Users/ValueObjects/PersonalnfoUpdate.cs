namespace Domain.Users.ValueObjects;

public sealed record PersonalInfoUpdate {
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public DateOnly? BirthDay { get; init; }
    public string? Country { get; init; }
    public string? Gender { get; init; }
}
