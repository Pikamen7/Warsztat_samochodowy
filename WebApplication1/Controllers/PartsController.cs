using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers;

public class PartsController : Controller
{
    private readonly AppDbContext _context;

    public PartsController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var parts = await _context.Parts
            .OrderBy(p => p.Name)
            .ToListAsync();

        return View(parts);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Part part)
    {
        if (!ModelState.IsValid)
        {
            return View(part);
        }

        CalculatePrices(part);

        _context.Parts.Add(part);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
    
    public async Task<IActionResult> Edit(int id)
    {
        var part = await _context.Parts.FindAsync(id);

        if (part == null)
        {
            return NotFound();
        }

        return View(part);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Part part)
    {
        if (id != part.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(part);
        }

        CalculatePrices(part);

        _context.Parts.Update(part);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var part = await _context.Parts.FindAsync(id);

        if (part == null)
        {
            return NotFound();
        }

        return View(part);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var part = await _context.Parts.FindAsync(id);

        if (part != null)
        {
            _context.Parts.Remove(part);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private static void CalculatePrices(Part part)
    {
        const decimal vatRate = 0.23m;

        part.PurchaseGrossPrice = Math.Round(part.PurchaseNetPrice * (1 + vatRate), 2);

        part.SaleNetPrice = Math.Round(
            part.PurchaseNetPrice * (1 + part.MarginPercent / 100),
            2
        );

        part.SaleGrossPrice = Math.Round(part.SaleNetPrice * (1 + vatRate), 2);
    }
}