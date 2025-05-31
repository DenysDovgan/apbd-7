using apbd_9.Models.Entities;

namespace apbd_9.Models.DTOs;

public class CreatePrescriptionDto
{
    public Patient Patient { get; set; } = null!;
    public List<MedicamentDto> Medicaments { get; set; } = new List<MedicamentDto>();
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
}