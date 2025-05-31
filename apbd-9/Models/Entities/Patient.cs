using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace apbd_9.Models.Entities;

[Table("Patient")]
public class Patient
{
    [Key]
    [Column("IdPatient")]
    public int Id { get; set; }
    
    [Required, MaxLength(100)]
    public string FirstName { get; set; } = null!;
    
    [Required, MaxLength(100)]
    public string LastName { get; set; } = null!;
    
    [Required]
    public DateTime BirthDate { get; set; }
    
    public ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
}