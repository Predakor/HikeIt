namespace Domain.FileReferences.ValueObjects;

public record class FileContent {
    public required string Name { get; init; }
    public required string Type { get; init; }
    public required long Size { get; init; }
    public required byte[] Content { get; init; }
}
