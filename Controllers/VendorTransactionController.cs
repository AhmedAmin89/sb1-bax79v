using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WarehouseSystem.Data;
using WarehouseSystem.Models;

namespace WarehouseSystem.Controllers
{
    public class VendorTransactionController : BaseController<VendorTransaction>
    {
        public VendorTransactionController(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<IActionResult> Index()
        {
            var transactions = await _dbSet
                .Include(t => t.Vendor)
                .Include(t => t.Warehouse)
                .Include(t => t.Item)
                .Where(t => !t.IsDeleted)
                .OrderByDescending(t => t.TransactionTime)
                .ToListAsync();
            return View(transactions);
        }

        public override async Task<IActionResult> Create()
        {
            ViewBag.Vendors = new SelectList(await _context.Vendors
                .Where(v => !v.IsDeleted)
                .OrderBy(v => v.VendorName)
                .ToListAsync(), "VendorId", "VendorName");

            ViewBag.Warehouses = new SelectList(await _context.Warehouses
                .Where(w => !w.IsDeleted)
                .OrderBy(w => w.WarehouseName)
                .ToListAsync(), "WarehouseId", "WarehouseName");

            ViewBag.Items = new SelectList(await _context.Items
                .Where(i => !i.IsDeleted)
                .OrderBy(i => i.ItemName)
                .ToListAsync(), "ItemId", "ItemName");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public override async Task<IActionResult> Create(VendorTransaction transaction)
        {
            if (ModelState.IsValid)
            {
                transaction.TransactionTime = DateTime.UtcNow;
                _dbSet.Add(transaction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            await PrepareViewBagForCreate();
            return View(transaction);
        }

        private async Task PrepareViewBagForCreate()
        {
            ViewBag.Vendors = new SelectList(await _context.Vendors
                .Where(v => !v.IsDeleted)
                .OrderBy(v => v.VendorName)
                .ToListAsync(), "VendorId", "VendorName");

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