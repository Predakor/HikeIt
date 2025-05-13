using HikeIt.Api.Repository;

namespace HikeIt.Api.Entities;

public class Peak : IRepositoryObject {
    public int Id { get; set; }
    public int Height { get; set; }

    public string? Name { get; set; }


    public Region Region { get; set; } //navigation prop
    public int RegionID { get; set; }
}
