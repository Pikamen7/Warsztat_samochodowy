using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers;

public class ServiceOrdersController : Controller
{
    private readonly AppDbContext _context;

    public ServiceOrdersController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var orders = await _context.ServiceOrders
            .Include(o => o.Customer)
            .Include(o => o.Items)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();

        return View(orders);
    }

    public async Task<IActionResult> Details(int id)
    {
        var order = await _context.ServiceOrders
            .Include(o => o.Customer)
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null)
        {
            return NotFound();
        }

        return View(order);
    }

    public async Task<IActionResult> Create()
    {
        ViewBag.Customers = await GetCustomerSelectList();
        ViewBag.Statuses = GetStatusSelectList();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ServiceOrder order)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Customers = await GetCustomerSelectList();
            ViewBag.Statuses = GetStatusSelectList();
            return View(order);
        }

        order.CreatedAt = DateTime.UtcNow;
        _context.ServiceOrders.Add(order);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Details), new { id = order.Id });
    }

    public async Task<IActionResult> Edit(int id)
    {
        var order = await _context.ServiceOrders
            .Include(o => o.Customer)
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null)
        {
            return NotFound();
        }

        ViewBag.Customers = await GetCustomerSelectList(order.CustomerId);
        ViewBag.Statuses = GetStatusSelectList(order.Status);
        return View(order);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ServiceOrder order)
    {
        if (id != order.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            ViewBag.Customers = await GetCustomerSelectList(order.CustomerId);
            ViewBag.Statuses = GetStatusSelectList(order.Status);
            order.Items = await _context.ServiceOrderItems
                .Where(i => i.ServiceOrderId == id)
                .ToListAsync();
            return View(order);
        }

        var existing = await _context.ServiceOrders.FindAsync(id);
        if (existing == null)
        {
            return NotFound();
        }

        existing.CustomerId = order.CustomerId;
        existing.Status = order.Status;
        existing.Description = order.Description;
        existing.LaborCost = order.LaborCost;

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Details), new { id });
    }

    public async Task<IActionResult> Delete(int id)
    {
        var order = await _context.ServiceOrders
            .Include(o => o.Customer)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null)
        {
            return NotFound();
        }

        return View(order);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var order = await _context.ServiceOrders.FindAsync(id);

        if (order != null)
        {
            _context.ServiceOrders.Remove(order);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddItem(int orderId, ServiceOrderItem item)
    {
        item.ServiceOrderId = orderId;
        item.TotalPrice = Math.Round(item.Quantity * item.UnitPrice, 2);

        if (ModelState.IsValid)
        {
            _context.ServiceOrderItems.Add(item);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Edit), new { id = orderId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RemoveItem(int itemId, int orderId)
    {
        var item = await _context.ServiceOrderItems.FindAsync(itemId);

        if (item != null)
        {
            _context.ServiceOrderItems.Remove(item);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Edit), new { id = orderId });
    }

    private async Task<SelectList> GetCustomerSelectList(int? selectedId = null)
    {
        var customers = await _context.Customers
            .OrderBy(c => c.LastName)
            .ThenBy(c => c.FirstName)
            .Select(c => new
            {
                c.Id,
                FullName = c.LastName + " " + c.FirstName + (c.RegistrationNumber != null ? " (" + c.RegistrationNumber + ")" : "")
            })
            .ToListAsync();

        return new SelectList(customers, "Id", "FullName", selectedId);
    }

    private SelectList GetStatusSelectList(ServiceOrderStatus? selected = null)
    {
        var statuses = new[]
        {
            new { Value = (int)ServiceOrderStatus.Nowe,       Text = "Nowe" },
            new { Value = (int)ServiceOrderStatus.WTrakcie,   Text = "W trakcie" },
            new { Value = (int)ServiceOrderStatus.Zakonczone, Text = "Zakończone" },
            new { Value = (int)ServiceOrderStatus.Anulowane,  Text = "Anulowane" }
        };

        return new SelectList(statuses, "Value", "Text", (int?)selected);
    }
}
