using System.ComponentModel.DataAnnotations;
using HikeIt.Api.Repository;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace HikeIt.Api.Entities;

public class Trip : IRepositoryObject
{
    public Guid Id { get; set; }

    [Required]
    public required float Height { get; set; }

    [Required]
    public required float Length { get; set; }

    [Required]
    public required float Duration { get; set; }

    public DateOnly TripDay { get; set; }

    //Navigation Property
    [ValidateNever]
    public Region Region { get; set; }

    public int RegionID { get; set; }
}

public class TripBuilder
{
    private float _height;
    private float _length;
    private float _duration;
    private DateOnly _tripDay;
    private int _regionId;

    public TripBuilder WithHeight(float height)
    {
        _height = height;
        return this;
    }

    public TripBuilder WithLength(float length)
    {
        _length = length;
        return this;
    }

    public TripBuilder WithDuration(float duration)
    {
        _duration = duration;
        return this;
    }

    public TripBuilder OnDay(DateOnly tripDay)
    {
        _tripDay = tripDay;
        return this;
    }

    public TripBuilder InRegion(int regionId)
    {
        _regionId = regionId;
        return this;
    }

    public Trip Build()
    {
        return new Trip
        {
            Height = _height,
            Length = _length,
            Duration = _duration,
            TripDay = _tripDay,
            RegionID = _regionId,
        };
    }
}
