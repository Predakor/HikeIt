namespace Domain.Common.ValueObjects;
public class FileReference {
    public string Url { get; private set; }
    public string FileName { get; private set; }
    public long Size { get; private set; }
    public string ContentType { get; private set; }

    public FileReference(string url, string fileName, long size, string contentType) {
        Url = url;
        FileName = fileName;
        Size = size;
        ContentType = contentType;
    }
}