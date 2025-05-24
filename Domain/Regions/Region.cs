using System.ComponentModel.DataAnnotations;

namespace Domain.Regions;

public class Region : IEntity {
    public int Id { get; set; }

    [Required]
    [MinLength(3, ErrorMessage = "region name to short")]
    [MaxLength(255, ErrorMessage = "region name to long")]
    public required string Name { get; set; }


}
