namespace Application.Users.Dtos;

public abstract record UserDataDto {
    public sealed record Personal {
        public required string FirstName { get; init; }
        public required string LastName { get; init; }
        public required string Email { get; init; }
        public required DateOnly BirthDay { get; init; }
        public required string Country { get; init; }
        public required string? Gender { get; init; }
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
}
