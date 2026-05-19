using Domain.Common.Abstractions;
using Domain.Peaks;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Domain.Locations.Regions;

public class Region : IEntity<int>
{
    public int Id { get; set; }

    [Required]
    [MinLength(3, ErrorMessage = "region name to short")]
    [MaxLength(255, ErrorMessage = "region name to long")]
    public required string Name { get; set; }

    public Collection<Peak> Peaks { get; init; } = [];
}
