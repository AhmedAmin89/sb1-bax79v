using System.ComponentModel.DataAnnotations;

namespace WarehouseSystem.Models
{
    public class Vendor : BaseEntity
    {
        [Key]
        public int VendorId { get; set; }

        [Required]
        [StringLength(100)]
        public string VendorName { get; set; }

        [Required]
        [StringLength(20)]
        public string Mobile { get; set; }

        [Required]
        [StringLength(200)]
        public string Address { get; set; }
    }
}