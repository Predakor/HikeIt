namespace Application.Dto;

public abstract record RegionDto {
    public record Basic(string Name) : RegionDto;
    public record Complete(int Id, string Name) : RegionDto;
    public record WithPeaks(Complete Region, List<PeakDto.Base> Peaks);
}
