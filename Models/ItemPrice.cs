using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WarehouseSystem.Models
{
    public class ItemPrice : BaseEntity
    {
        [Key]
        public int ItemPriceId { get; set; }

        [ForeignKey("Item")]
        public int ItemId { get; set; }
        public virtual Item Item { get; set; }

        [ForeignKey("PriceList")]
        public int PriceListId { get; set; }
        public virtual PriceList PriceList { get; set; }

        [ForeignKey("UnitOfMeasure")]
        public int UOMId { get; set; }
        public virtual UnitOfMeasure UnitOfMeasure { get; set; }

        [Required]
        public decimal Price { get; set; }
    }
}