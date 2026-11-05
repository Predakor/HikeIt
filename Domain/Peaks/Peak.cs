using Domain.Common.AggregateRoot;
using Domain.Common.Geography.Factories;
using Domain.Common.Geography.ValueObjects;
using Domain.Locations.Regions;
using NetTopologySuite.Geometries;

namespace Domain.Peaks;

public class Peak : AggregateRoot<int, Peak>, IGeoPoint
{
    public required string Name { get; set; }
    public required int Height { get; set; }
    public required Point Location { get; set; }

    public double Lat => Location.Y;
    public double Lon => Location.X;
    public double Ele => Height;

    public int RegionID { get; set; }

    //navigation prop
    public Region Region { get; set; } = default!;

    public Peak UpdateRegion(int regionID)
    {
        if (regionID != RegionID)
        {
            RegionID = regionID;
            //emit region change event so region progression can update
        }

        return this;
    }

    public static Peak Create(string name, IGeoPoint point, int regionId)
    {
        return new Peak()
        {
            Id = default,
            Name = name,
            Height = (int)point.Ele,
            Location = GeoFactory.CreatePoint(point.Lon, point.Lat),
            RegionID = regionId,
        };
    }

}