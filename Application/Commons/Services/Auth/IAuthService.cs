using Application.Users.Root.Dtos;
using Domain.Users.Root;

namespace Application.Commons.Services.Auth;

public interface IAuthService
{
    /// <summary>
/// Retrieves the currently logged-in user.
/// </summary>
/// <returns>A Result containing the authenticated <see cref="User"/> on success, or a failure Result otherwise.</returns>
Task<Result<User>> WithLoggedUser();
    /// <summary>
/// Gets the identifier of the currently logged-in user.
/// </summary>
/// <returns>A Result containing the logged-in user's GUID identifier.</returns>
Task<Result<Guid>> WithLoggedUserId();

    /// <summary>
/// Retrieve a user identified by either their login or email.
/// </summary>
/// <param name="loginOrEmail">The user's login name or email address to search for.</param>
/// <returns>A Result containing the matched <see cref="User"/>, or a failure result if no matching user is found or an error occurs.</returns>
Task<Result<User>> GetByLoginOrEmail(string loginOrEmail);
    /// <summary>
/// Reset a user's password using the provided reset request.
/// </summary>
/// <param name="request">The reset-password request DTO containing the data required to verify the user and set the new password.</param>
/// <returns>`true` if the password was successfully reset, `false` otherwise.</returns>
Task<Result<bool>> ResetPassword(UserDto.ResetPassword request);
    /// <summary>
/// Starts the password reset process for the specified email address.
/// </summary>
/// <param name="email">The email address of the user requesting a password reset.</param>
/// <returns>The password reset token when the operation succeeds; otherwise an error message.</returns>
Task<Result<string>> ForgotPassword(string email);

}
