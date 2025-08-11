namespace Application.Dto;

public abstract record RegionDto {
    public record Basic(int Id, string Name) : RegionDto;
    public record Complete(int Id, string Name) : RegionDto;
    public record WithPeaks(Complete Region, List<PeakDto.Base> Peaks);
    public record WithDetailedPeaks(Complete Region, List<PeakDto.WithLocation> Peaks);

}
