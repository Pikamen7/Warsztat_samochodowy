using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models;

public class Customer
{
    public int Id { get; set; }

    [Required]
    [Display(Name = "Imię")]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Nazwisko")]
    public string LastName { get; set; } = string.Empty;

    [Display(Name = "Telefon")]
    public string? PhoneNumber { get; set; }

    [Display(Name = "Email")]
    public string? Email { get; set; }

    [Display(Name = "Marka auta")]
    public string? CarBrand { get; set; }

    [Display(Name = "Model auta")]
    public string? CarModel { get; set; }

    [Display(Name = "Numer rejestracyjny")]
    public string? RegistrationNumber { get; set; }

    [Display(Name = "Notatki")]
    public string? Notes { get; set; }
}