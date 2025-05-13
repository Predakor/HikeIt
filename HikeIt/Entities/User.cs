using HikeIt.Api.Repository;

namespace HikeIt.Api.Entities;

public class User : IRepositoryObject {
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public DateOnly BirthDay { get; set; }
}
