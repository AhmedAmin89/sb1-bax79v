using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WarehouseSystem.Data;
using WarehouseSystem.Models;

namespace WarehouseSystem.Controllers
{
    public class UnitOfMeasureController : BaseController<UnitOfMeasure>
    {
        public UnitOfMeasureController(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<IActionResult> Index()
        {
            var uoms = await _dbSet
                .Where(u => !u.IsDeleted)
                .OrderBy(u => u.UomName)
                .ToListAsync();
            return View(uoms);
        }
    }
}