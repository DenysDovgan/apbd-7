namespace apbd_9.Models.DTOs;

public class PatientDetailsDto
{
    public int IdPatient { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public DateTime BirthDate { get; set; }
    public List<PrescriptionSummaryDto> Prescriptions { get; set; } = new();
}