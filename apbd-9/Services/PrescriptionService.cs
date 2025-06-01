using apbd_9.Data;
using apbd_9.Models.DTOs;
using apbd_9.Models.Entities;

namespace apbd_9.Services;

public interface IPrescriptionService
{
    public Task<Prescription> CreatePrescriptionAsync(CreatePrescriptionDto createDto);
}

public class PrescriptionService : IPrescriptionService
{
    private readonly AppDbContext _context;

    public PrescriptionService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Prescription> CreatePrescriptionAsync(CreatePrescriptionDto dto)
    {
        if (dto.Medicaments.Count > 10)
            throw new Exception("Prescription cannot contain more than 10 medicaments.");

        if (dto.DueDate < dto.Date)
            throw new Exception("DueDate must be greater than or equal to Date.");

        var missingMedicament = dto.Medicaments
            .FirstOrDefault(m => !_context.Medicaments.Any(x => x.Id == m.IdMedicament));

        if (missingMedicament != null)
            throw new Exception($"Medicament with id {missingMedicament.IdMedicament} does not exist.");

        var patient = await _context.Patients.FindAsync(dto.Patient.IdPatient);
        if (patient == null)
        {
            patient = new Patient
            {
                Id = dto.Patient.IdPatient,
                FirstName = dto.Patient.FirstName,
                LastName = dto.Patient.LastName,
                BirthDate = dto.Patient.BirthDate
            };
            _context.Patients.Add(patient);
        }

        var prescription = new Prescription
        {
            DoctorId = dto.DoctorId,
            Patient = patient,
            Date = dto.Date,
            DueDate = dto.DueDate,
            PrescriptionMedicaments = dto.Medicaments.Select(m => new PrescriptionMedicament
            {
                MedicamentId = m.IdMedicament,
                Dose = m.Dose,
                Details = m.Description
            }).ToList()
        };

        _context.Prescriptions.Add(prescription);
        await _context.SaveChangesAsync();

        return prescription;
    }
}