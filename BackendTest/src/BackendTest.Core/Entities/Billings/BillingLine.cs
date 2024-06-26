using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendTest.Entities.Billings
{
    public class BillingLine : AuditedEntity<Guid>
    {
        [Required]
        public Guid BillingId { get; set; }

        [Required]
        public Guid ProductId { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }
        
        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal UnitPrice { get; set; }

        public decimal Subtotal => Quantity * UnitPrice;

        public Billing Billing { get; set; }
    }
}
