using System.ComponentModel.DataAnnotations;

namespace WarehouseSystem.Models
{
    public class UnitOfMeasure : BaseEntity
    {
        [Key]
        public int UomId { get; set; }

        [Required]
        [StringLength(50)]
        public string UomName { get; set; }
    }
}