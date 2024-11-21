using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WarehouseSystem.Data;
using WarehouseSystem.Models;

namespace WarehouseSystem.Controllers
{
    public class VendorController : BaseController<Vendor>
    {
        public VendorController(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<IActionResult> Index()
        {
            var vendors = await _dbSet
                .Where(v => !v.IsDeleted)
                .OrderBy(v => v.VendorName)
                .ToListAsync();
            return View(vendors);
        }
    }
}