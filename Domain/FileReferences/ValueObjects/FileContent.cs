namespace Domain.FileReferences.ValueObjects;

public record FileContent(string Name, string Type, long Size, byte[] Content);
