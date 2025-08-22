using Domain.FileReferences;
using Domain.FileReferences.ValueObjects;
using Microsoft.AspNetCore.Http;

namespace Application.Commons.Files;

public static class FileDataExtentios {
    public static async Task<byte[]> GetFileContent(this IFormFile file) {
        using var ms = new MemoryStream();
        await file.CopyToAsync(ms);
        return ms.ToArray();
    }

    public static async Task<FileContent> ToFileContent(this IFormFile file) {
        var content = await file.GetFileContent();
        return new(
            Name: file.FileName,
            Type: file.ContentType,
            Size: file.Length,
            Content: content
        );
    }

    public static FileReference ToFileReference(this IFormFile file, Guid id, string? storageName) {
        var reference = new FileReference() {
            Id = id,
            FileName = file.FileName,
            ContentType = file.ContentType,
            Size = file.Length,
        };

        if (storageName is not null) {
            reference.SetStorageName(storageName);
        }

        return reference;
    }
}
