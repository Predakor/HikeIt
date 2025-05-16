using HikeIt.Api.Repository;

namespace HikeIt.Api.Entities;

public class User : IRepositoryObject {
    public int Id { get; set; }
    public required string Name { get; set; }
    public string Email { get; set; }
    public DateOnly BirthDay { get; set; }
}
