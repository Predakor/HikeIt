using Application.Commons.FileStorage;
using Application.Commons.Options;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Domain.Common;
using Domain.Common.Result;
using Domain.Common.ValueObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Infrastructure.Storage;

internal class AzureBlobStorage : IFileStorage {
    readonly BlobContainerClient _filesContainer;
    readonly BlobContainerClient _avatarContainer;

    public AzureBlobStorage(IServiceProvider provider) {
        var storageOptions =
            provider.GetRequiredService<IOptions<StorageOptions>>().Value
            ?? throw new Exception("Storage options now found check your env variables");

        var credentials = new StorageSharedKeyCredential(
            storageOptions.Account,
            storageOptions.Key
        );

        var blobUri = $"https://{storageOptions.Account}.blob.core.windows.net";
        var blobClient = new BlobServiceClient(new Uri(blobUri), credentials);

        _filesContainer = blobClient.GetBlobContainerClient("user-files");
        _avatarContainer = blobClient.GetBlobContainerClient("user-avatars");
    }

    BlobClient GetClient(string path, BlobContainer container) {
        var blobContainer = container switch {
            BlobContainer.Avatar => _avatarContainer,
            BlobContainer.File => _filesContainer,
            _ => throw new Exception("Unsuported Blob type"),
        };

        return blobContainer.GetBlobClient(path.ToString());
    }

    public async Task<Result<Stream>> DownloadAsync(string path, BlobContainer container) {
        BlobClient file = GetClient(path, container);

        if (!await file.ExistsAsync()) {
            return Errors.NotFound("file", path);
        }

        return await file.OpenReadAsync();
    }

    public async Task<Result<FileReference>> UploadAsync(
        IFormFile file,
        string path,
        BlobContainer container
    ) {
        BlobClient client = GetClient(path, container);

        await using (Stream? data = file.OpenReadStream()) {
            await client.UploadAsync(
                data,
                new BlobUploadOptions {
                    HttpHeaders = new BlobHttpHeaders {
                        ContentType = file.ContentType,
                        CacheControl = "public, max-age=86400",
                    },
                }
            );
        }

        return new FileReference(
            client.Uri.AbsoluteUri,
            file.FileName,
            file.Length,
            file.ContentType
        );
    }

    public Task<Result<FileReference>> UpdateAsync(string path, BlobContainer container) {
        throw new NotImplementedException();
    }

    public async Task<Result<bool>> DeleteAsync(string path, BlobContainer container) {
        BlobClient client = GetClient(path, container);

        var result = await client.DeleteIfExistsAsync();
        return result.Value;
    }
}
