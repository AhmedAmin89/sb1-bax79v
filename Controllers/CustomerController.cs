using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WarehouseSystem.Data;
using WarehouseSystem.Models;

namespace WarehouseSystem.Controllers
{
    public class CustomerController : BaseController<Customer>
    {
        public CustomerController(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<IActionResult> Index()
        {
            var customers = await _dbSet
                .Include(c => c.PriceList)
                .Where(c => !c.IsDeleted)
                .OrderBy(c => c.CustomerName)
                .ToListAsync();
            return View(customers);
        }

        public override async Task<IActionResult> Create()
        {
            ViewBag.PriceLists = new SelectList(await _context.PriceLists
                .Where(p => !p.IsDeleted)
                .OrderBy(p => p.PriceListName)
                .ToListAsync(), "PriceListId", "PriceListName");

            return View();
        }
    }
}