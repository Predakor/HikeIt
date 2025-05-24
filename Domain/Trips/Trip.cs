using Domain.Regions;
using System.ComponentModel.DataAnnotations;

namespace Domain.Trips;

public class Trip : IEntity {
    public int Id { get; set; }

    [Required]
    public required float Height { get; set; }

    [Required]
    public required float Distance { get; set; }

    [Required]
    public required float Duration { get; set; }
    public int RegionID { get; set; }

    public DateOnly TripDay { get; set; }

    //Navigation Property
    public Region Region { get; set; }

}
