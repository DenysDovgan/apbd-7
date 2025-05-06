using apbd_7.Models;
using apbd_7.Models.DTOs.Client;
using apbd_7.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace apbd_7.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientsController(IClientRepository clientRepository, ITripRepository tripRepository) : ControllerBase
{

    // I've implemented this endpoint just for adding more functionalities
    [HttpGet("{id}")]
    public async Task<IActionResult> GetClientById(int id)
    {
        var client = await clientRepository.GetClientByIdAsync(id);
        if (client == null)
            return NotFound($"Client with id {id} not found");
        
        return Ok(client);
    }

    [HttpGet("{id}/trips")]
    public async Task<IActionResult> GetClientTrips(int id)
    {
        if (!await clientRepository.ClientExistsAsync(id))
            return NotFound($"Client with id {id} not found");
        
        var trips = await clientRepository.GetClientTripsAsync(id);
        if (!trips.Any())
            return NotFound("Client is not registered for any trips");
        
        return Ok(trips);
    }

    [HttpPost]
    public async Task<IActionResult> AddClient([FromBody] CreateClientDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        Client client = await clientRepository.AddClientAsync(dto);
        
        return CreatedAtAction(nameof(GetClientById), new { id = client.IdClient }, client);
    }

    [HttpPut("{id}/trip/{tripId}")]
    public async Task<IActionResult> RegisterToTrip(int id, int tripId)
    {
        if (!await clientRepository.ClientExistsAsync(id))
            return NotFound($"Client with id {id} does not exist");
        
        if (!await tripRepository.TripExistsAsync(tripId))
            return NotFound($"Trip with id {tripId} does not exist");
        
        var result = await clientRepository.RegisterClientToTripAsync(id, tripId);
        if (!result)
            return BadRequest("Registration failed");
        
        return Ok("Client successfully registered");
    }

    [HttpDelete("{id}/trip/{tripId}")]
    public async Task<IActionResult> UnregisterFromTrip(int id, int tripId)
    {
        if (!await clientRepository.IsClientRegisteredAsync(id, tripId))
            return NotFound("Registration does not exist");

        var result = await clientRepository.RemoveClientFromTripAsync(id, tripId);
        if (!result)
            return BadRequest("Unregistration failed");

        return Ok("Client unregistered successfully");
    }
}