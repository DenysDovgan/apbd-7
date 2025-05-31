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
    public async Task<IActionResult> Create([FromBody] CreatePrescriptionDto createDto)
    {
        var prescription = await prescriptionService.CreateAsync(createDto);
        return CreatedAtAction(nameof(Create), new { id = prescription.Id }, prescription);
    }
}