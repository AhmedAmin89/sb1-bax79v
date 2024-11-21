using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WarehouseSystem.Data;
using WarehouseSystem.Models;

namespace WarehouseSystem.Controllers
{
    public class PriceListController : BaseController<PriceList>
    {
        public PriceListController(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<IActionResult> Index()
        {
            var priceLists = await _dbSet
                .Where(p => !p.IsDeleted)
                .OrderBy(p => p.PriceListName)
                .ToListAsync();
            return View(priceLists);
        }
    }
}