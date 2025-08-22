using Domain.Common.Abstractions;
using Domain.FileReferences.ValueObjects;

namespace Domain.FileReferences;

public class FileReference : IEntity<Guid> {
    public required Guid Id { get; init; }
    public required long Size { get; init; }
    public required string FileName { get; init; }
    public required string ContentType { get; init; }
    public string StorageName { get; private set; } = string.Empty;
    public string Url { get; private set; } = string.Empty;

    public readonly DateTime CreatedAt = DateTime.UtcNow;

    public static FileReference FromFileContent(FileContent content, string storageName, Guid id) {
        return new FileReference() {
            Id = id,
            Size = content.Size,
            FileName = content.Name,
            ContentType = content.Type,
            StorageName = storageName,
        };
    }

    public FileReference SetUrl(string url) {
        if (url is not null) {
            Url = url;
        }
        return this;
    }

    public FileReference SetStorageName(string name) {
        if (name is not null) {
            StorageName = name;
        }
        return this;
    }
}
