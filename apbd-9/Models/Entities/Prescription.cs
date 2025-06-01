using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace apbd_9.Models.Entities;

[Table("Prescription")]
public class Prescription
{
    [Key]
    [Column("IdPrescription")]
    public int Id { get; set; }

    [Required] 
    public DateTime Date { get; set; }
    
    [Required]
    public DateTime DueDate { get; set; }
    
    [ForeignKey(nameof (Patient))]
    [Column("IdPatient")]
    public int PatientId { get; set; }
    
    [ForeignKey(nameof (Doctor))]
    [Column("IdDoctor")]
    public int DoctorId { get; set; }
    
    public Patient Patient { get; set; } = null!;
    public Doctor Doctor { get; set; } = null!;
    
    public ICollection<PrescriptionMedicament> PrescriptionMedicaments { get; set; } = new List<PrescriptionMedicament>();
}