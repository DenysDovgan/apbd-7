using apbd_9.Data;
using apbd_9.Models.Entities;

namespace apbd_9.Services;

public interface IPatientService
{
    public Task<Patient> GetByIdAsync(int id);
}

public class PatientService
{
    private readonly AppDbContext _context;
    
    public PatientService(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<Patient> GetByIdAsync(int id)
    {
        var patient = await _context.Patients.FindAsync(id);
        if (patient == null)
        {
            throw new KeyNotFoundException($"Patient with ID {id} not found.");
        }
        return patient;
    }
}