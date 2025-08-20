namespace Domain.Common.ValueObjects;

public class FileReference {
    public long Size { get; init; }
    public required string FileName { get; init; }
    public required string ContentType { get; init; }
    public string StorageName { get; private set; } = string.Empty;
    public string Url { get; private set; } = string.Empty;

    public static FileReference FromFileContent(FileContent content) {
        return new FileReference() {
            Size = content.Size,
            FileName = content.FileName,
            ContentType = content.ContentType,
            StorageName = content.StorageName,
        };
    }

    public FileReference SetUrl(string url) {
        if (url is not null) {
            Url = url;
        }
        return this;
    }

    public FileReference SetStorageName(string url) {
        if (url is not null) {
            Url = url;
        }
        return this;
    }
}
