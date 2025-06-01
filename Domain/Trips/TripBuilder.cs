namespace Domain.Trips;

public class TripBuilder {
    private float _height;
    private float _length;
    private float _duration;
    private DateOnly _tripDay;
    private int _regionId;

    public TripBuilder WithHeight(float height) {
        _height = height;
        return this;
    }

    public TripBuilder WithLength(float length) {
        _length = length;
        return this;
    }

    public TripBuilder WithDuration(float duration) {
        _duration = duration;
        return this;
    }

    public TripBuilder OnDay(DateOnly tripDay) {
        _tripDay = tripDay;
        return this;
    }

    public TripBuilder InRegion(int regionId) {
        _regionId = regionId;
        return this;
    }

    public Trip Build() {
        return new Trip {
            Height = _height,
            Distance = _length,
            Duration = _duration,
            TripDay = _tripDay,
            RegionId = _regionId,
        };
    }
}
