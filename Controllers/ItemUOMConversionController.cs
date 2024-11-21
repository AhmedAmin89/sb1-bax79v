using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WarehouseSystem.Data;
using WarehouseSystem.Models;

namespace WarehouseSystem.Controllers
{
    public class ItemUOMConversionController : BaseController<ItemUOMConversion>
    {
        public ItemUOMConversionController(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<IActionResult> Index()
        {
            var conversions = await _dbSet
                .Include(c => c.Item)
                .Include(c => c.FromUOM)
                .Include(c => c.ToUOM)
                .Where(c => !c.IsDeleted)
                .OrderBy(c => c.Item.ItemName)
                .ToListAsync();
            return View(conversions);
        }

        public override async Task<IActionResult> Create()
        {
            ViewBag.Items = new SelectList(await _context.Items
                .Where(i => !i.IsDeleted)
                .OrderBy(i => i.ItemName)
                .ToListAsync(), "ItemId", "ItemName");

            ViewBag.UOMs = new SelectList(await _context.UnitOfMeasures
                .Where(u => !u.IsDeleted)
                .OrderBy(u => u.UomName)
                .ToListAsync(), "UomId", "UomName");

            return View();
        }
    }
}