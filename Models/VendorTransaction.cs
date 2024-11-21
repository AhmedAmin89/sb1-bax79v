using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WarehouseSystem.Models
{
    public class VendorTransaction : BaseEntity
    {
        [Key]
        public int TransactionId { get; set; }

        [ForeignKey("Vendor")]
        public int VendorId { get; set; }
        public virtual Vendor Vendor { get; set; }

        [ForeignKey("Warehouse")]
        public int WarehouseId { get; set; }
        public virtual Warehouse Warehouse { get; set; }

        [ForeignKey("Item")]
        public int ItemId { get; set; }
        public virtual Item Item { get; set; }

        [Required]
        public decimal Quantity { get; set; }

        [Required]
        public DateTime TransactionTime { get; set; }
    }
}