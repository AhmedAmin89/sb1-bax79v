using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WarehouseSystem.Data;
using WarehouseSystem.Models;

namespace WarehouseSystem.Controllers
{
    public class ItemController : BaseController<Item>
    {
        public ItemController(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<IActionResult> Index()
        {
            var items = await _dbSet
                .Include(i => i.DefaultUOM)
                .Include(i => i.LargeUOM)
                .Include(i => i.SmallUOM)
                .Where(i => !i.IsDeleted)
                .OrderBy(i => i.ItemName)
                .ToListAsync();
            return View(items);
        }

        public override async Task<IActionResult> Create()
        {
            var uoms = await _context.UnitOfMeasures
                .Where(u => !u.IsDeleted)
                .OrderBy(u => u.UomName)
                .ToListAsync();

            ViewBag.UOMs = new SelectList(uoms, "UomId", "UomName");
            return View();
        }

        public override async Task<IActionResult> Edit(int id)
        {
            var item = await _dbSet
                .Include(i => i.DefaultUOM)
                .Include(i => i.LargeUOM)
                .Include(i => i.SmallUOM)
                .FirstOrDefaultAsync(i => i.ItemId == id && !i.IsDeleted);

            if (item == null)
            {
                return NotFound();
            }

            var uoms = await _context.UnitOfMeasures
                .Where(u => !u.IsDeleted)
                .OrderBy(u => u.UomName)
                .ToListAsync();

            ViewBag.UOMs = new SelectList(uoms, "UomId", "UomName");
            return View(item);
        }
    }
}