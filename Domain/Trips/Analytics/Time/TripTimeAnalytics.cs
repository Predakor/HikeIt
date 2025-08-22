namespace Domain.Trips.Analytics.Time;
//Owned Type
public class TimeAnalytic {
    public TimeSpan Duration { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }

    public TimeSpan ActiveTime { get; set; }
    public TimeSpan IdleTime { get; set; }
    public TimeSpan AscentTime { get; set; }
    public TimeSpan DescentTime { get; set; }

    public double AverageSpeedKph { get; set; }
    public double AverageAscentKph { get; set; }
    public double AverageDescentKph { get; set; }
}
