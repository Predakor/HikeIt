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

    public async Task<Result<string>> Upload(IFormFile file, User user) {
        string path = user.Id.ToString();
        var fileContent = await file.ToFileContent(path);

        return await FileValidator
            .ValidateAvatar(file)
            .BindAsync(_ => _fileStorage.UploadAsync(fileContent, path, Container))
            .MapAsync(f => user.SetAvatar(f.Url))
            .TapAsync(_ => _userRepository.SaveChangesAsync())
            .MapAsync(u => u.Avatar!);
    }
}
