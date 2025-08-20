using Domain.Interfaces;

namespace Domain.FileReferences;

public interface IFileReferenceRepository : IReadResultRepository<FileReference, Guid> { }
