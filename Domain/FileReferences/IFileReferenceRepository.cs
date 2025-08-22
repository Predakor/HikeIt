using Domain.Common.Abstractions;

namespace Domain.FileReferences;

public interface IFileReferenceRepository : IReadResultRepository<FileReference, Guid> { }
