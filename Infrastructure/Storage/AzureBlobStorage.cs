using Application.Commons.FileStorage;
using Application.Commons.Options;
using Azure.Storage;
using Azure.Storage.Blobs;
using Domain.Common;
using Domain.Common.Result;
using Domain.Common.ValueObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Infrastructure.Storage;

internal class AzureBlobStorage : IFileStorage {
    readonly BlobContainerClient _filesContainer;

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
    }

    public Task<Result<string>> GetFileUrlAsync(Guid id) {
        throw new NotImplementedException();
    }

    public async Task<Result<Stream>> DownloadAsync(Guid id) {
        BlobClient file = _filesContainer.GetBlobClient(id.ToString());

        if (!await file.ExistsAsync()) {
            return Errors.NotFound("file", id);
        }

        return await file.OpenReadAsync();
    }

    public async Task<Result<FileReference>> UploadAsync(IFormFile file, Guid id) {
        BlobClient client = _filesContainer.GetBlobClient(id.ToString());

        await using (Stream? data = file.OpenReadStream()) {
            await client.UploadAsync(data);
        }

        return new FileReference(client.Uri.AbsoluteUri, file.FileName, file.Length, file.ContentType);
    }

    public Task<Result<FileReference>> UpdateAsync(Guid id) {
        throw new NotImplementedException();
    }

    public Task<Result<bool>> DeleteAsync(Guid id) {
        throw new NotImplementedException();
    }
}
