using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WarehouseSystem.Data;
using WarehouseSystem.Models;

namespace WarehouseSystem.Controllers
{
    public class ItemPriceController : BaseController<ItemPrice>
    {
        public ItemPriceController(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<IActionResult> Index()
        {
            var prices = await _dbSet
                .Include(p => p.Item)
                .Include(p => p.PriceList)
                .Include(p => p.UnitOfMeasure)
                .Where(p => !p.IsDeleted)
                .OrderBy(p => p.PriceList.PriceListName)
                .ThenBy(p => p.Item.ItemName)
                .ToListAsync();
            return View(prices);
        }

        public override async Task<IActionResult> Create()
        {
            ViewBag.Items = new SelectList(await _context.Items
                .Where(i => !i.IsDeleted)
                .OrderBy(i => i.ItemName)
                .ToListAsync(), "ItemId", "ItemName");

            ViewBag.PriceLists = new SelectList(await _context.PriceLists
                .Where(p => !p.IsDeleted)
                .OrderBy(p => p.PriceListName)
                .ToListAsync(), "PriceListId", "PriceListName");

            ViewBag.UOMs = new SelectList(await _context.UnitOfMeasures
                .Where(u => !u.IsDeleted)
                .OrderBy(u => u.UomName)
                .ToListAsync(), "UomId", "UomName");

            return View();
        }
    }
}