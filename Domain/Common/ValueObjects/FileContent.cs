namespace Domain.Common.ValueObjects;

public record class FileContent {
    public required string FileName { get; init; }
    public required string ContentType { get; init; }
    public required long Size { get; init; }
    public required byte[] Content { get; init; }
    public required string StorageName { get; init; }
}
