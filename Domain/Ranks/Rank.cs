using Domain.Interfaces;

namespace Domain.Ranks;

public class Rank : IEntity<int> {
    public int Id { get; init; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public int RequiredKms { get; set; }
    public int RequiredPeaks { get; set; }
}
