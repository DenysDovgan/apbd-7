using apbd_7.Models;
using apbd_7.Models.DTOs.Client;
using apbd_7.Models.DTOs.Trip;

namespace apbd_7.Repositories.Interfaces;

public interface IClientRepository
{
    Task<Client> AddClientAsync(CreateClientDto dto);
    Task<List<ClientTripsResponseDto>> GetClientTripsAsync(int clientId);
    Task<bool> RegisterClientToTripAsync(int clientId, int tripId);
    Task<bool> RemoveClientFromTripAsync(int clientId, int tripId);
    Task<bool> ClientExistsAsync(int clientId);
    Task<bool> IsClientRegisteredAsync(int clientId, int tripId);
    Task<Client?> GetClientByIdAsync(int clientId);
}