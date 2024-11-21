using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WarehouseSystem.Models
{
    public class Invoice : BaseEntity
    {
        [Key]
        public int InvoiceId { get; set; }

        [Required]
        public DateTime DateTime { get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        [Required]
        public decimal InvoiceTotal { get; set; }

        public decimal ManualDiscount { get; set; }

        [Required]
        public decimal TotalAfterDiscount { get; set; }

        public virtual ICollection<InvoiceLine> InvoiceLines { get; set; }
    }

    public class InvoiceLine : BaseEntity
    {
        [Key]
        public int InvoiceLineId { get; set; }

        [ForeignKey("Invoice")]
        public int InvoiceId { get; set; }
        public virtual Invoice Invoice { get; set; }

        [ForeignKey("Item")]
        public int ItemId { get; set; }
        public virtual Item Item { get; set; }

        [ForeignKey("UnitOfMeasure")]
        public int UOMId { get; set; }
        public virtual UnitOfMeasure UnitOfMeasure { get; set; }

        [Required]
        public decimal UnitPrice { get; set; }

        [Required]
        public decimal Quantity { get; set; }
    }
}