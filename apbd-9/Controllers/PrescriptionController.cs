using System.ComponentModel.DataAnnotations;
using apbd_9.Models.DTOs;
using apbd_9.Services;
using Microsoft.AspNetCore.Mvc;

namespace apbd_9.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PrescriptionController(IPrescriptionService prescriptionService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePrescriptionDto dto)
    {
        try
        {
            var id = await prescriptionService.CreatePrescriptionAsync(dto);
            return Created($"api/prescription/{id}", new { id });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}