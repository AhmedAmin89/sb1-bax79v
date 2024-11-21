using WarehouseSystem.Models;

namespace WarehouseSystem.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalItems { get; set; }
        public int TotalCustomers { get; set; }
        public int TotalVendors { get; set; }
        public int TotalWarehouses { get; set; }
        public List<WarehouseTransaction> RecentTransactions { get; set; }
        public List<Invoice> RecentInvoices { get; set; }
    }
}