using System.ComponentModel.DataAnnotations;
using HikeIt.Api.Repository;

namespace HikeIt.Api.Entities;

public class Region : IRepositoryObject
{
    public int Id { get; set; }

    [Required]
    public required string Name { get; set; }
}
