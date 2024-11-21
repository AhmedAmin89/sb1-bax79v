using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WarehouseSystem.Models
{
    public class Customer : BaseEntity
    {
        [Key]
        public int CustomerId { get; set; }

        [Required]
        [StringLength(100)]
        public string CustomerName { get; set; }

        [Required]
        [StringLength(20)]
        public string Mobile { get; set; }

        [Required]
        [StringLength(200)]
        public string Address { get; set; }

        [ForeignKey("PriceList")]
        public int PriceListId { get; set; }
        public virtual PriceList PriceList { get; set; }
    }
}