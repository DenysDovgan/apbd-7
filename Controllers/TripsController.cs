using apbd_7.Models;
using apbd_7.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace apbd_7.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TripsController(ITripRepository tripRepository) : ControllerBase
{

    [HttpGet]
    public async Task<IActionResult> GetAllTrips()
    {
        var trips = await tripRepository.GetAllTripsWithCountriesAsync();
        return Ok(trips);
    }
}