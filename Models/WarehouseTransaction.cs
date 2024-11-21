using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WarehouseSystem.Models
{
    public class WarehouseTransaction : BaseEntity
    {
        [Key]
        public int TransactionId { get; set; }

        [ForeignKey("FromWarehouse")]
        public int FromWarehouseId { get; set; }
        public virtual Warehouse FromWarehouse { get; set; }

        [ForeignKey("ToWarehouse")]
        public int ToWarehouseId { get; set; }
        public virtual Warehouse ToWarehouse { get; set; }

        [ForeignKey("Item")]
        public int ItemId { get; set; }
        public virtual Item Item { get; set; }

        [Required]
        public decimal Quantity { get; set; }

        [Required]
        public DateTime TransactionTime { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}