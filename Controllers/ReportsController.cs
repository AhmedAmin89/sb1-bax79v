using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WarehouseSystem.Data;
using WarehouseSystem.ViewModels;

namespace WarehouseSystem.Controllers
{
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReportsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> StockReport()
        {
            var stockByWarehouse = await _context.Items
                .Where(i => !i.IsDeleted)
                .Select(item => new StockReportViewModel
                {
                    ItemId = item.ItemId,
                    ItemName = item.ItemName,
                    DefaultUOM = item.DefaultUOM.UomName,
                    WarehouseStock = _context.Warehouses
                        .Where(w => !w.IsDeleted)
                        .Select(warehouse => new WarehouseStockViewModel
                        {
                            WarehouseName = warehouse.WarehouseName,
                            Stock = CalculateStock(item.ItemId, warehouse.WarehouseId)
                        })
                        .ToList()
                })
                .ToListAsync();

            return View(stockByWarehouse);
        }

        private decimal CalculateStock(int itemId, int warehouseId)
        {
            // Calculate inbound stock (from vendors)
            var inboundStock = _context.VendorTransactions
                .Where(t => !t.IsDeleted && t.ItemId == itemId && t.WarehouseId == warehouseId)
                .Sum(t => t.Quantity);

            // Calculate transfers in
            var transfersIn = _context.WarehouseTransactions
                .Where(t => !t.IsDeleted && t.ItemId == itemId && t.ToWarehouseId == warehouseId)
                .Sum(t => t.Quantity);

            // Calculate transfers out
            var transfersOut = _context.WarehouseTransactions
                .Where(t => !t.IsDeleted && t.ItemId == itemId && t.FromWarehouseId == warehouseId)
                .Sum(t => t.Quantity);

            // Calculate sales (invoices)
            var sales = _context.InvoiceLines
                .Where(l => !l.IsDeleted && l.ItemId == itemId)
                .Join(_context.Invoices.Where(i => !i.IsDeleted),
                    line => line.InvoiceId,
                    invoice => invoice.InvoiceId,
                    (line, invoice) => line.Quantity)
                .Sum();

            return inboundStock + transfersIn - transfersOut - sales;
        }

        public async Task<IActionResult> TransactionHistory(int? itemId, int? warehouseId, DateTime? startDate, DateTime? endDate)
        {
            var query = _context.WarehouseTransactions
                .Include(t => t.Item)
                .Include(t => t.FromWarehouse)
                .Include(t => t.ToWarehouse)
                .Include(t => t.User)
                .Where(t => !t.IsDeleted);

            if (itemId.HasValue)
                query = query.Where(t => t.ItemId == itemId);

            if (warehouseId.HasValue)
                query = query.Where(t => t.FromWarehouseId == warehouseId || t.ToWarehouseId == warehouseId);

            if (startDate.HasValue)
                query = query.Where(t => t.TransactionTime >= startDate);

            if (endDate.HasValue)
                query = query.Where(t => t.TransactionTime <= endDate);

            ViewBag.Items = await _context.Items.Where(i => !i.IsDeleted).ToListAsync();
            ViewBag.Warehouses = await _context.Warehouses.Where(w => !w.IsDeleted).ToListAsync();

            var transactions = await query
                .OrderByDescending(t => t.TransactionTime)
                .ToListAsync();

            return View(transactions);
        }
    }
}