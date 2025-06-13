using Domain.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entiites.Regions;

public class Region : IEntity<int> {
    public int Id { get; set; }

    [Required]
    [MinLength(3, ErrorMessage = "region name to short")]
    [MaxLength(255, ErrorMessage = "region name to long")]
    public required string Name { get; set; }


}
