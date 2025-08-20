using Domain.Common.ValueObjects;
using Microsoft.AspNetCore.Http;

namespace Application.Commons.FileStorage;

public static class FileDataExtentios {
    public static async Task<byte[]> GetFileContent(this IFormFile file) {
        using var ms = new MemoryStream();
        await file.CopyToAsync(ms);
        return ms.ToArray();
    }

    public static async Task<FileContent> ToFileContent(this IFormFile file, string storageName) {
        var content = await file.GetFileContent();
        return new() {
            Content = content,
            ContentType = file.ContentType,
            FileName = file.FileName,
            Size = file.Length,
            StorageName = storageName,
        };
    }
}
