using apbd_9.Data;
using apbd_9.Models.DTOs;
using apbd_9.Models.Entities;

namespace apbd_9.Services;

public interface IPrescriptionService
{
    public Task<Prescription> CreateAsync(CreatePrescriptionDto createDto);
}

public class PrescriptionService : IPrescriptionService
{
    private readonly AppDbContext _context;
    private readonly IPatientService _patientService;
    
    public PrescriptionService(AppDbContext context, IPatientService patientService)
    {
        _context = context;
        _patientService = patientService;
    }
    
    public async Task<Prescription> CreateAsync(CreatePrescriptionDto createDto)
    {
        var patient = createDto.Patient;
    }
}