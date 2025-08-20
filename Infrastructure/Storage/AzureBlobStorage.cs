using Application.Commons.FileStorage;
using Application.Commons.Options;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Domain.Common;
using Domain.Common.Result;
using Domain.Common.ValueObjects;
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

    public async Task<Result<Stream>> DownloadAsync(string path, BlobContainer container) {
        BlobClient file = GetClient(path, container);

        if (!await file.ExistsAsync()) {
            return Errors.NotFound("file", path);
        }

        return await file.OpenReadAsync();
    }

    public async Task<Result<FileReference>> UploadAsync(
        FileContent file,
        string path,
        BlobContainer type
    ) {
        BlobClient client = GetClient(path, type);

        Console.WriteLine(client.Uri.AbsolutePath);
        Console.WriteLine(client.Uri.AbsolutePath);
        Console.WriteLine(client.Uri.AbsolutePath);

        using var stream = new MemoryStream(file.Content);
        await UploadToBlob(client, stream, file.ContentType);

        return FileReference.FromFileContent(file).SetUrl(client.Uri.AbsoluteUri);
    }

    public async Task<Result<bool>> DeleteAsync(string path, BlobContainer container) {
        Console.WriteLine(path);
        Console.WriteLine(path);
        Console.WriteLine(path);
        Console.WriteLine(path);
        Console.WriteLine(path);

        var request = await GetClient(path, container).DeleteIfExistsAsync();
        return Result<bool>.Success(request.Value);

    }

    static async Task UploadToBlob(BlobClient client, Stream data, string contentType) {
        BlobUploadOptions options = new UploadOptions()
            .WithCache(24)
            .WithContentType(contentType)
            .Finalize();

        await client.UploadAsync(data, options);
    }

    BlobClient GetClient(string path, BlobContainer container) {
        var blobContainer = container switch {
            BlobContainer.Avatar => _avatarContainer,
            BlobContainer.File => _filesContainer,
            _ => throw new Exception("Unsuported Blob type"),
        };

        return blobContainer.GetBlobClient(path.ToString());
    }
}

class UploadOptions() {
    readonly BlobHttpHeaders headers = new();

    public UploadOptions WithCache(uint hours, bool publicCache = false) {
        string accesPolicy = publicCache ? "public" : "private";
        headers.CacheControl = $"{accesPolicy}, max-age={hours * 60 * 60}";
        return this;
    }

    public UploadOptions WithContentType(string contenType) {
        headers.ContentType = contenType;
        return this;
    }

    public BlobUploadOptions Finalize() {
        return new BlobUploadOptions { HttpHeaders = headers };
    }
}
