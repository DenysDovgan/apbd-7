using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace apbd_9.Models.Entities;

[Table( "Medicament" )]
public class Medicament
{
    [Key]
    [Column( "IdMedicament" )]
    public int Id { get; set; }
    
    [Required, MaxLength( 100 )]
    public string Name { get; set; } = null!;
    
    [Required, MaxLength( 100 )]
    public string Description { get; set; } = null!;
    
    [Required, MaxLength( 100 )]
    public string Type { get; set; } = null!;
    
    public ICollection<PrescriptionMedicament> PrescriptionMedicaments { get; set; } = new List<PrescriptionMedicament>();
}