using apbd_9.Data;
using apbd_9.Models.DTOs;
using apbd_9.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace apbd_9.Services;

public interface IPatientService
{
    public Task<PatientDetailsDto?> GetPatientDetailsAsync(int id);
}

public class PatientService : IPatientService
{
    private readonly AppDbContext _context;

    public PatientService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PatientDetailsDto?> GetPatientDetailsAsync(int id)
    {
        var patient = await _context.Patients
            .Include(p => p.Prescriptions)
            .ThenInclude(pr => pr.Doctor)
            .Include(p => p.Prescriptions)
            .ThenInclude(pr => pr.PrescriptionMedicaments)
            .ThenInclude(pm => pm.Medicament)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (patient == null) return null;

        return new PatientDetailsDto
        {
            IdPatient = patient.Id,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            BirthDate = patient.BirthDate,
            Prescriptions = patient.Prescriptions
                .OrderBy(p => p.DueDate)
                .Select(p => new PrescriptionSummaryDto
                {
                    IdPrescription = p.Id,
                    Date = p.Date,
                    DueDate = p.DueDate,
                    Doctor = new DoctorDto
                    {
                        IdDoctor = p.Doctor.Id,
                        FirstName = p.Doctor.FirstName,
                        LastName = p.Doctor.LastName,
                        Email = p.Doctor.Email
                    },
                    Medicaments = p.PrescriptionMedicaments.Select(pm => new MedicamentSummaryDto
                    {
                        IdMedicament = pm.Medicament.Id,
                        Name = pm.Medicament.Name,
                        Description = pm.Medicament.Description,
                        Dose = pm.Dose
                    }).ToList()
                }).ToList()
        };
    }
}