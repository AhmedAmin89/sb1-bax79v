using Microsoft.EntityFrameworkCore;
using WarehouseSystem.Data;
using WarehouseSystem.Models;

namespace WarehouseSystem.Services
{
    public class UOMConversionService
    {
        private readonly ApplicationDbContext _context;

        public UOMConversionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<decimal> ConvertQuantity(int itemId, int fromUOMId, int toUOMId, decimal quantity)
        {
            if (fromUOMId == toUOMId)
                return quantity;

            var conversion = await _context.ItemUOMConversions
                .FirstOrDefaultAsync(c => 
                    c.ItemId == itemId && 
                    c.FromUOMId == fromUOMId && 
                    c.ToUOMId == toUOMId && 
                    !c.IsDeleted);

            if (conversion != null)
                return quantity * conversion.ConversionRate;

            // Try reverse conversion
            conversion = await _context.ItemUOMConversions
                .FirstOrDefaultAsync(c => 
                    c.ItemId == itemId && 
                    c.FromUOMId == toUOMId && 
                    c.ToUOMId == fromUOMId && 
                    !c.IsDeleted);

            if (conversion != null)
                return quantity / conversion.ConversionRate;

            throw new InvalidOperationException("No conversion rate found between these units of measure.");
        }

        public async Task ValidateStock(int itemId, int warehouseId, int uomId, decimal quantity)
        {
            var item = await _context.Items
                .Include(i => i.DefaultUOM)
                .FirstOrDefaultAsync(i => i.ItemId == itemId && !i.IsDeleted);

            if (item == null)
                throw new InvalidOperationException("Item not found.");

            // Convert quantity to default UOM
            var quantityInDefaultUOM = await ConvertQuantity(itemId, uomId, item.DefaultUOMId, quantity);

            // Calculate current stock in default UOM
            var currentStock = await CalculateStock(itemId, warehouseId);

            if (currentStock < quantityInDefaultUOM)
                throw new InvalidOperationException($"Insufficient stock. Available: {currentStock} {item.DefaultUOM.UomName}");
        }

        private async Task<decimal> CalculateStock(int itemId, int warehouseId)
        {
            // Implementation similar to ReportsController.CalculateStock
            var inboundStock = await _context.VendorTransactions
                .Where(t => !t.IsDeleted && t.ItemId == itemId && t.WarehouseId == warehouseId)
                .SumAsync(t => t.Quantity);

            var transfersIn = await _context.WarehouseTransactions
                .Where(t => !t.IsDeleted && t.ItemId == itemId && t.ToWarehouseId == warehouseId)
                .SumAsync(t => t.Quantity);

            var transfersOut = await _context.WarehouseTransactions
                .Where(t => !t.IsDeleted && t.ItemId == itemId && t.FromWarehouseId == warehouseId)
                .SumAsync(t => t.Quantity);

            var sales = await _context.InvoiceLines
                .Where(l => !l.IsDeleted && l.ItemId == itemId)
                .Join(_context.Invoices.Where(i => !i.IsDeleted),
                    line => line.InvoiceId,
                    invoice => invoice.InvoiceId,
                    (line, invoice) => line.Quantity)
                .SumAsync();

            return inboundStock + transfersIn - transfersOut - sales;
        }
    }
}