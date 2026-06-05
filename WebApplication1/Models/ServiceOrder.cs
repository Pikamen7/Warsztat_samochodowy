using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models;

public enum ServiceOrderStatus
{
    [Display(Name = "Nowe")]
    Nowe = 0,

    [Display(Name = "W trakcie")]
    WTrakcie = 1,

    [Display(Name = "Zakończone")]
    Zakonczone = 2,

    [Display(Name = "Anulowane")]
    Anulowane = 3
}

public class ServiceOrder
{
    public int Id { get; set; }

    [Required]
    [Display(Name = "Klient")]
    public int CustomerId { get; set; }

    public Customer? Customer { get; set; }

    [Display(Name = "Data przyjęcia")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Display(Name = "Status")]
    public ServiceOrderStatus Status { get; set; } = ServiceOrderStatus.Nowe;

    [Display(Name = "Opis prac")]
    public string? Description { get; set; }

    [Display(Name = "Koszt robocizny netto")]
    public decimal LaborCost { get; set; }

    public List<ServiceOrderItem> Items { get; set; } = new();
}
