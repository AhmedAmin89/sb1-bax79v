namespace WarehouseSystem.ViewModels
{
    public class StockReportViewModel
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string DefaultUOM { get; set; }
        public List<WarehouseStockViewModel> WarehouseStock { get; set; }
        public decimal TotalStock => WarehouseStock?.Sum(w => w.Stock) ?? 0;
    }

    public class WarehouseStockViewModel
    {
        public string WarehouseName { get; set; }
        public decimal Stock { get; set; }
    }
}