using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models;

public class Part
{
    public int Id { get; set; }

    [Required]
    [Display(Name = "Nazwa części")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Numer katalogowy")]
    public string? CatalogNumber { get; set; }

    [Display(Name = "Cena zakupu netto")]
    public decimal PurchaseNetPrice { get; set; }

    [Display(Name = "Cena zakupu brutto")]
    public decimal PurchaseGrossPrice { get; set; }

    [Display(Name = "Marża %")]
    public decimal MarginPercent { get; set; }

    [Display(Name = "Cena sprzedaży netto")]
    public decimal SaleNetPrice { get; set; }

    [Display(Name = "Cena sprzedaży brutto")]
    public decimal SaleGrossPrice { get; set; }

    [Display(Name = "Ilość na magazynie")]
    public int QuantityInStock { get; set; }

    [Display(Name = "Notatki")]
    public string? Notes { get; set; }
}