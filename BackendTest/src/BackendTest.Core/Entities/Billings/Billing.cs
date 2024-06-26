using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendTest.Entities.Billings
{
    public class Billing : AuditedAggregateRoot<Guid>
    {
        [Required]
        [StringLength(100)]
        public string InvoiceNumber { get; set; }

        [Required]
        public Guid CustomerId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public DateTime DueDate { get; set; }
        public decimal TotalAmount { get; set; }

        [Required]
        [StringLength(3)]
        public string Currency { get; set; }

        public ICollection<BillingLine> Lines { get; private set; }

        public Billing()
        {
            Lines = new List<BillingLine>();
        }
    }
}
