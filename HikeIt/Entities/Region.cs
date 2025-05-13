using HikeIt.Api.Repository;
using System.ComponentModel.DataAnnotations;

namespace HikeIt.Api.Entities;

public class Region : IRepositoryObject {
    public int Id { get; set; }

    [Required]
    public required string Name { get; set; }

}
