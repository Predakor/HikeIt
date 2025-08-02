namespace Application.ReachedPeaks.Dtos;

public record ReachedPeakDto(string Name, int Height, DateTime? ReachedAt = null);
