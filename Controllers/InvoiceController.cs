using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WarehouseSystem.Data;
using WarehouseSystem.Models;
using WarehouseSystem.Services;

namespace WarehouseSystem.Controllers
{
    public class InvoiceController : BaseController<Invoice>
    {
        private readonly UOMConversionService _uomConversionService;

        public InvoiceController(
            ApplicationDbContext context,
            ICurrentUserService currentUserService,
            UOMConversionService uomConversionService) 
            : base(context, currentUserService)
        {
            _uomConversionService = uomConversionService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public override async Task<IActionResult> Create(Invoice invoice)
        {
            if (ModelState.IsValid)
            {
                // Validate stock for each line
                foreach (var line in invoice.InvoiceLines)
                {
                    await _uomConversionService.ValidateStock(
                        line.ItemId,
                        1, // Assuming default warehouse ID
                        line.UOMId,
                        line.Quantity);
                }

                invoice.DateTime = DateTime.UtcNow;
                _dbSet.Add(invoice);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            await PrepareViewBagForCreate();
            return View(invoice);
        }

        private async Task PrepareViewBagForCreate()
        {
            ViewBag.Customers = new SelectList(await _context.Customers
                .Where(c => !c.IsDeleted)
                .OrderBy(c => c.CustomerName)
                .ToListAsync(), "CustomerId", "CustomerName");
        }
    }
}