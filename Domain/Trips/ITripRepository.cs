namespace Domain.Trips;

public interface ITripRepository {
    public Task<Trip> GetAsync(int id);
    public Task<List<Trip>> GetAllAsync();
    public Task Add(Trip trip);
    public Task Update(Trip trip);
    public Task Delete(Trip trip);
}