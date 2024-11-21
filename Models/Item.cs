using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WarehouseSystem.Models
{
    public class Item : BaseEntity
    {
        [Key]
        public int ItemId { get; set; }

        [Required]
        [StringLength(100)]
        public string ItemName { get; set; }

        [ForeignKey("DefaultUOM")]
        public int DefaultUOMId { get; set; }
        public virtual UnitOfMeasure DefaultUOM { get; set; }

        [ForeignKey("LargeUOM")]
        public int LargeUOMId { get; set; }
        public virtual UnitOfMeasure LargeUOM { get; set; }

        [ForeignKey("SmallUOM")]
        public int SmallUOMId { get; set; }
        public virtual UnitOfMeasure SmallUOM { get; set; }
    }
}