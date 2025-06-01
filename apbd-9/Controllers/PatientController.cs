using apbd_9.Services;
using Microsoft.AspNetCore.Mvc;

namespace apbd_9.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientController(IPatientService patientService) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var result = await patientService.GetPatientDetailsAsync(id);
        return result is null ? NotFound() : Ok(result);
    }
}