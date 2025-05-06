using apbd_7.Models;
using apbd_7.Repositories.Interfaces;
using Microsoft.Data.SqlClient;

namespace apbd_7.Repositories;

public class TripRepository : ITripRepository
{
    private readonly string _connectionString;

    public TripRepository(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("Default");
    }

    public async Task<List<Trip>> GetAllTripsWithCountriesAsync()
    {
        var trips = new List<Trip>();

        const string tripQuery = @"
        SELECT IdTrip, Name, Description, DateFrom, DateTo, MaxPeople
        FROM Trip";

        const string countriesQuery = @"
        SELECT ct.IdTrip, c.IdCountry, c.Name
        FROM Country_Trip ct
        JOIN Country c ON c.IdCountry = ct.IdCountry";

        using var conn = new SqlConnection(_connectionString);
        await conn.OpenAsync();

        // First load all trips
        using (var cmd = new SqlCommand(tripQuery, conn))
        using (var reader = await cmd.ExecuteReaderAsync())
        {
            while (await reader.ReadAsync())
            {
                trips.Add(new Trip
                {
                    IdTrip = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Description = reader.GetString(2),
                    DateFrom = reader.GetDateTime(3),
                    DateTo = reader.GetDateTime(4),
                    MaxPeople = reader.GetInt32(5),
                    Countries = new List<Country>()
                });
            }
        }

        // Then load countries and assign to trips
        using (var cmd = new SqlCommand(countriesQuery, conn))
        using (var reader = await cmd.ExecuteReaderAsync())
        {
            while (await reader.ReadAsync())
            {
                int tripId = reader.GetInt32(0);
                var country = new Country
                {
                    IdCountry = reader.GetInt32(1),
                    Name = reader.GetString(2)
                };

                var trip = trips.FirstOrDefault(t => t.IdTrip == tripId);
                trip?.Countries?.Add(country);
            }
        }

        return trips;
    }

    public async Task<bool> TripExistsAsync(int tripId)
    {
        const string query = "SELECT COUNT(1) FROM Trip WHERE IdTrip = @Id";

        using var conn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@Id", tripId);
        await conn.OpenAsync();

        var result = await cmd.ExecuteScalarAsync();
        return Convert.ToInt32(result) > 0;
    }
}