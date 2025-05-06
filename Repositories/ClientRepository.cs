using apbd_7.Models;
using apbd_7.Models.DTOs.Client;
using apbd_7.Models.DTOs.Trip;
using apbd_7.Repositories.Interfaces;
using Microsoft.Data.SqlClient;

namespace apbd_7.Repositories;

public class ClientRepository : IClientRepository
{
    private readonly string _connectionString;

    public ClientRepository(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("Default");
    }

    public async Task<Client> AddClientAsync(CreateClientDto dto)
    {
        const string query = @"
            INSERT INTO Client (FirstName, LastName, Email, Telephone, Pesel)
            VALUES (@FirstName, @LastName, @Email, @Telephone, @Pesel);
            SELECT SCOPE_IDENTITY();";

        using var conn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@FirstName", dto.FirstName);
        cmd.Parameters.AddWithValue("@LastName", dto.LastName);
        cmd.Parameters.AddWithValue("@Email", dto.Email);
        cmd.Parameters.AddWithValue("@Telephone", dto.Telephone);
        cmd.Parameters.AddWithValue("@Pesel", dto.Pesel);

        await conn.OpenAsync();
        var id = Convert.ToInt32(await cmd.ExecuteScalarAsync());

        return new Client
        {
            IdClient = id,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            Telephone = dto.Telephone,
            Pesel = dto.Pesel
        };
    }

    public async Task<bool> RegisterClientToTripAsync(int clientId, int tripId)
    {
        const string query = @"
            INSERT INTO Client_Trip (IdClient, IdTrip, RegisteredAt)
            VALUES (@ClientId, @TripId, GETDATE());";

        using var conn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@ClientId", clientId);
        cmd.Parameters.AddWithValue("@TripId", tripId);

        await conn.OpenAsync();
        return await cmd.ExecuteNonQueryAsync() > 0;
    }

    public async Task<bool> RemoveClientFromTripAsync(int clientId, int tripId)
    {
        const string query = @"
            DELETE FROM Client_Trip
            WHERE IdClient = @ClientId AND IdTrip = @TripId;";

        using var conn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@ClientId", clientId);
        cmd.Parameters.AddWithValue("@TripId", tripId);

        await conn.OpenAsync();
        return await cmd.ExecuteNonQueryAsync() > 0;
    }

    public async Task<bool> ClientExistsAsync(int clientId)
    {
        const string query = "SELECT COUNT(1) FROM Client WHERE IdClient = @Id";

        using var conn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@Id", clientId);
        await conn.OpenAsync();

        var result = await cmd.ExecuteScalarAsync();
        return Convert.ToInt32(result) > 0;
    }

    public async Task<bool> IsClientRegisteredAsync(int clientId, int tripId)
    {
        const string query = @"
            SELECT COUNT(1) FROM Client_Trip
            WHERE IdClient = @ClientId AND IdTrip = @TripId";

        using var conn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@ClientId", clientId);
        cmd.Parameters.AddWithValue("@TripId", tripId);
        await conn.OpenAsync();

        var result = await cmd.ExecuteScalarAsync();
        return Convert.ToInt32(result) > 0;
    }

    public async Task<Client?> GetClientByIdAsync(int clientId)
    {
        const string query = @"
        SELECT IdClient, FirstName, LastName, Email, Telephone, Pesel
        FROM Client
        WHERE IdClient = @ClientId";

        using var conn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@ClientId", clientId);

        await conn.OpenAsync();
        using var reader = await cmd.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            return new Client
            {
                IdClient = reader.GetInt32(0),
                FirstName = reader.GetString(1),
                LastName = reader.GetString(2),
                Email = reader.GetString(3),
                Telephone = reader.GetString(4),
                Pesel = reader.GetString(5)
            };
        }

        return null;
    }
    
    public async Task<List<ClientTripsResponseDto>> GetClientTripsAsync(int clientId)
    {
        var result = new List<ClientTripsResponseDto>();

        const string query = @"
        SELECT t.IdTrip, t.Name, t.Description, t.DateFrom, t.DateTo, t.MaxPeople,
               ct.RegisteredAt, ct.PaymentDate
        FROM Trip t
        INNER JOIN Client_Trip ct ON t.IdTrip = ct.IdTrip
        WHERE ct.IdClient = @ClientId";

        using var conn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@ClientId", clientId);

        await conn.OpenAsync();
        using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            result.Add(new ClientTripsResponseDto
            {
                IdTrip = reader.GetInt32(0),
                Name = reader.GetString(1),
                Description = reader.GetString(2),
                DateFrom = reader.GetDateTime(3),
                DateTo = reader.GetDateTime(4),
                MaxPeople = reader.GetInt32(5),
                RegisteredAt = reader.GetInt32(6),
                PaymentDate = reader.IsDBNull(7) ? null : reader.GetInt32(7)
            });
        }

        return result;
    }
}