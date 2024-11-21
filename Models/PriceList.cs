using System.ComponentModel.DataAnnotations;

namespace WarehouseSystem.Models
{
    public class PriceList : BaseEntity
    {
        [Key]
        public int PriceListId { get; set; }

        [Required]
        [StringLength(100)]
        public string PriceListName { get; set; }
    }
}