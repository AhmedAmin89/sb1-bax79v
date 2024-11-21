using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WarehouseSystem.Models
{
    public class ItemUOMConversion : BaseEntity
    {
        [Key]
        public int ConversionId { get; set; }

        [ForeignKey("Item")]
        public int ItemId { get; set; }
        public virtual Item Item { get; set; }

        [ForeignKey("FromUOM")]
        public int FromUOMId { get; set; }
        public virtual UnitOfMeasure FromUOM { get; set; }

        [ForeignKey("ToUOM")]
        public int ToUOMId { get; set; }
        public virtual UnitOfMeasure ToUOM { get; set; }

        [Required]
        public decimal ConversionRate { get; set; }
    }
}