using Domain.Users.Root;
using static Application.Users.Root.Dtos.UserDataDto;

namespace Application.Users.Root.Dtos;

public abstract record UserDto(string UserName)
{
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
    public sealed record ResetPassword(string UserName, string Token, string Password);
}

public static class UsersDtoExtentions
{
    /// <summary>
    /// Create a public-facing profile from a User entity.
    /// </summary>
    /// <param name="user">The source User entity to convert into a public profile.</param>
    /// <returns>The public-facing profile populated with UserName, Avatar, Rank (defaults to &quot;Novice Hiker&quot; if missing), Peaks, Trips, and Traveled distance (numeric fields default to 0).</returns>
    public static PublicProfile ToPublicProfile(this User user)
    {
        return new()
        {
            UserName = user.UserName!,
            Avatar = user.Avatar,
            Rank = user?.Rank?.Name ?? "Novice Hiker",
            Peaks = user?.Stats.TotalPeaks ?? 0,
            Trips = user?.Stats.TotalTrips ?? 0,
            Traveled = user?.Stats.TotalDistanceMeters ?? 0,
        };
    }

    /// <summary>
    /// Converts a <see cref="User"/> into a <see cref="Personal"/> DTO.
    /// </summary>
    /// <param name="user">The source user to convert.</param>
    /// <returns>
    /// A <see cref="Personal"/> populated with BirthDay, Country (empty string if null), Email (empty string if null), FirstName, LastName, and Gender as a string.
    /// </returns>
    public static Personal ToPersonal(this User user)
    {
        return new()
        {
            BirthDay = user.BirthDay,
            Country = user.Country ?? "",
            Email = user.Email ?? "",
            FirstName = user.FirstName,
            LastName = user.LastName,
            Gender = user.Gender.ToString(),
        };
    }

    /// <summary>
    /// Creates an AccountState containing the user's account metadata.
    /// </summary>
    /// <param name="user">The source User whose account metadata will be copied.</param>
    /// <returns>An AccountState with CreatedAt taken from the user, Role set to "user", and Status set to "active".</returns>
    public static AccountState ToAccountState(this User user)
    {
        return new()
        {
            CreatedAt = user.CreatedAt,
            Role = "user",
            Status = "active",
        };
    }

    /// <summary>
    /// Create a UserDto.Basic representing the user's basic public information.
    /// </summary>
    /// <param name="user">The source User whose UserName and Avatar populate the DTO.</param>
    /// <param name="roles">An array of role names to include in the DTO.</param>
    /// <returns>A UserDto.Basic containing the user's UserName, the provided roles, and the user's Avatar.</returns>
    public static UserDto.Basic ToBasic(this User user, string[] roles)
    {
        return new(UserName: user.UserName!, Roles: roles, Avatar: user.Avatar);
    }
}

public static class UserDtoFactory
{
    /// <summary>
    /// Creates a Complete UserDto populated from the specified User.
    /// </summary>
    /// <returns>A UserDto.Complete populated with the user's first name, last name, username (empty string if null), email (empty string if null), and birth date.</returns>
    public static UserDto.Complete ToComplete(this User user)
    {
        return new(
            user.FirstName,
            user.LastName,
            user.UserName ?? "",
            user.Email ?? "",
            user.BirthDay
        );
    }
}
