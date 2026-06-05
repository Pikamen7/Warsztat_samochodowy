using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models;

public class ServiceOrderItem
{
    public int Id { get; set; }

    public int ServiceOrderId { get; set; }

    public ServiceOrder? ServiceOrder { get; set; }

    [Required]
    [Display(Name = "Nazwa części / usługi")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Ilość")]
    public int Quantity { get; set; } = 1;

    [Display(Name = "Cena jedn. netto")]
    public decimal UnitPrice { get; set; }

    [Display(Name = "Wartość netto")]
    public decimal TotalPrice { get; set; }
}
