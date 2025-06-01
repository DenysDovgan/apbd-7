namespace apbd_9.Models.DTOs;

public class CreatePrescriptionDto
{
    public PatientDto Patient { get; set; } = null!;

    public int DoctorId { get; set; }

    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }

    public List<PrescriptionMedicamentDto> Medicaments { get; set; } = new();
}