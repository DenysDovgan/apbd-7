using apbd_7.Models;

namespace apbd_7.Repositories.Interfaces;

public interface ITripRepository
{
    Task<List<Trip>> GetAllTripsWithCountriesAsync();
    Task<bool> TripExistsAsync(int tripId);
}