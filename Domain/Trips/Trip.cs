using Domain.GpxFiles;
using Domain.Regions;
using Domain.TrackAnalytics;
using Domain.Users;

namespace Domain.Trips;

public class Trip : IEntity {
    public int Id { get; set; }
    public required float Height { get; set; }
    public required float Distance { get; set; }
    public required float Duration { get; set; }
    public DateOnly TripDay { get; set; }

    //Owned Type
    public TrackAnalytic? TrackAnalytics { get; set; }

    //Foreign Keys
    public int RegionID { get; set; }
    public Guid? GpxFileID { get; set; }
    public int UserId { get; set; }

    //Navigation Property
    public Region? Region { get; set; }
    public GpxFile? GpxFile { get; set; }
    public User? User { get; set; }

}
