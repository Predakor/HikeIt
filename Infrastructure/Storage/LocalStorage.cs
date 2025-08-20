using Application.Commons.FileStorage;
using Domain.Common;
using Domain.Common.Result;
using Domain.Common.ValueObjects;

namespace Infrastructure.Storage;

internal class LocalStorage : IFileStorage {
    const string rooot = "wwwroot";

    static string GetContainerFolder(BlobContainer container) {
        return container switch {
            BlobContainer.Avatar => "files",
            BlobContainer.File => "avatars",
            _ => throw new Exception("Unsuported container type"),
        };
    }

    static string GetUploadFolder(string path, BlobContainer container) {
        string folder = GetContainerFolder(container);

        return Path.Combine(rooot, folder, path.ToString());
    }

    public Task<Result<Stream>> DownloadAsync(string path, BlobContainer type) {
        throw new NotImplementedException();
    }

    public Task<Result<FileReference>> UpdateAsync(string path, BlobContainer type) {
        throw new NotImplementedException();
    }

    public async Task<Result<FileReference>> UploadAsync(
        FileContent file,
        string path,
        BlobContainer container
    ) {
        try {
            var uploadDir = GetUploadFolder(path, container);

            Directory.CreateDirectory(uploadDir);

            Console.WriteLine("Saving file in location: " + uploadDir);
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var fullPath = Path.Combine(uploadDir, fileName);

            await File.WriteAllBytesAsync(fullPath, file.Content);

            return FileReference.FromFileContent(file).SetUrl(fullPath).SetStorageName(fileName);
        }
        catch (Exception ex) {
            var error = Errors.Unknown($"Could not save file: {ex.Message}");
            return error;
        }
    }

    public Task<Result<bool>> DeleteAsync(string path, BlobContainer type) {
        throw new NotImplementedException();
    }
}
