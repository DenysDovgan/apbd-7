using System.ComponentModel.DataAnnotations;

namespace apbd_7.Models.DTOs.Client;

public class CreateClientDto
{
    [Required]
    public string FirstName { get; set; } = null!;
    
    [Required]
    public string LastName { get; set; } = null!;
    
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;
    
    [Required]
    [Phone]
    public string Telephone { get; set; } = null!;
    
    [Required]
    [StringLength(11, MinimumLength = 11)]
    [RegularExpression(@"^\d{11}$", ErrorMessage = "PESEL must be exactly 11 digits")]
    public string Pesel { get; set; } = null!;
}