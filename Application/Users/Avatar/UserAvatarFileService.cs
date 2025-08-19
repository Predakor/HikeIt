using Application.Commons.FileStorage;
using Domain.Common.Result;
using Domain.Users;
using Microsoft.AspNetCore.Http;

namespace Application.Users.Avatar;

internal class UserAvatarFileService : IUserAvatarFileService {
    const BlobContainer Container = BlobContainer.Avatar;

    readonly IFileStorage _fileStorage;
    readonly IUserRepository _userRepository;

    public UserAvatarFileService(IFileStorage fileStorage, IUserRepository userRepository) {
        _fileStorage = fileStorage;
        _userRepository = userRepository;
    }

    public Task<Result<bool>> Delete(User user) {
        return _fileStorage.DeleteAsync(user.Id.ToString(), Container);
    }

    public Task<Result<string>> Upload(IFormFile file, User user) {
        return ValidateUserAvatar(file)
            .BindAsync(_ => _fileStorage.UploadAsync(file, user.Id.ToString(), Container))
            .MapAsync(f => user.SetAvatar(f.Url))
            .TapAsync(_ => _userRepository.SaveChangesAsync())
            .MapAsync(u => u.Avatar!);
    }

    static Result<IFormFile> ValidateUserAvatar(IFormFile file) {
        return new FileValidator(file)
            .HasMaxSizeMB(0.5)
            .HasValidExtention([".png", ".jpg", ".webp", ".avif"])
            .Validate();
    }
}
