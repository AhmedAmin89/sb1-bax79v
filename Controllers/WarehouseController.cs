using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WarehouseSystem.Data;
using WarehouseSystem.Models;

namespace WarehouseSystem.Controllers
{
    public class WarehouseController : BaseController<Warehouse>
    {
        public WarehouseController(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<IActionResult> Index()
        {
            var warehouses = await _dbSet
                .Where(w => !w.IsDeleted)
                .OrderBy(w => w.WarehouseName)
                .ToListAsync();
            return View(warehouses);
        }
    }
}