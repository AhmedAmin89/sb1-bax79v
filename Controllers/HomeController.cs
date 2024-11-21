using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WarehouseSystem.Data;
using WarehouseSystem.ViewModels;

namespace WarehouseSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var dashboard = new DashboardViewModel
            {
                TotalItems = await _context.Items.CountAsync(i => !i.IsDeleted),
                TotalCustomers = await _context.Customers.CountAsync(c => !c.IsDeleted),
                TotalVendors = await _context.Vendors.CountAsync(v => !v.IsDeleted),
                TotalWarehouses = await _context.Warehouses.CountAsync(w => !w.IsDeleted),
                
                RecentTransactions = await _context.WarehouseTransactions
                    .Include(t => t.Item)
                    .Include(t => t.FromWarehouse)
                    .Include(t => t.ToWarehouse)
                    .Where(t => !t.IsDeleted)
                    .OrderByDescending(t => t.TransactionTime)
                    .Take(5)
                    .ToListAsync(),
                
                RecentInvoices = await _context.Invoices
                    .Include(i => i.Customer)
                    .Where(i => !i.IsDeleted)
                    .OrderByDescending(i => i.DateTime)
                    .Take(5)
                    .ToListAsync()
            };

            return View(dashboard);
        }
    }
}