using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WarehouseSystem.Data;
using WarehouseSystem.Models;
using WarehouseSystem.Services;

namespace WarehouseSystem.Controllers
{
    public class WarehouseTransactionController : BaseController<WarehouseTransaction>
    {
        private readonly UOMConversionService _uomConversionService;

        public WarehouseTransactionController(
            ApplicationDbContext context,
            ICurrentUserService currentUserService,
            UOMConversionService uomConversionService) 
            : base(context, currentUserService)
        {
            _uomConversionService = uomConversionService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public override async Task<IActionResult> Create(WarehouseTransaction transaction)
        {
            if (ModelState.IsValid)
            {
                // Validate stock
                await _uomConversionService.ValidateStock(
                    transaction.ItemId,
                    transaction.FromWarehouseId,
                    transaction.Item.DefaultUOMId,
                    transaction.Quantity);

                transaction.TransactionTime = DateTime.UtcNow;
                transaction.UserId = _currentUserService.GetCurrentUserId();
                _dbSet.Add(transaction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            await PrepareViewBagForCreate();
            return View(transaction);
        }

        private async Task PrepareViewBagForCreate()
        {
            ViewBag.Warehouses = new SelectList(await _context.Warehouses
                .Where(w => !w.IsDeleted)
                .OrderBy(w => w.WarehouseName)
                .ToListAsync(), "WarehouseId", "WarehouseName");

            ViewBag.Items = new SelectList(await _context.Items
                .Where(i => !i.IsDeleted)
                .OrderBy(i => i.ItemName)
                .ToListAsync(), "ItemId", "ItemName");
        }
    }
}