using System.ComponentModel.DataAnnotations;

namespace WarehouseSystem.Models
{
    public class Warehouse : BaseEntity
    {
        [Key]
        public int WarehouseId { get; set; }

        [Required]
        [StringLength(100)]
        public string WarehouseName { get; set; }
    }
}