namespace Application.Dto;

public abstract record PeakDto(int Height, string Name) {
    public record Simple(int Height, string Name, int RegionID) : PeakDto(Height, Name);
    public record Complete(int Height, string Name, RegionDto.Complete Region) : PeakDto(Height, Name);

    public record Reached(int Id, int Height, DateTime? Time = null);
}

