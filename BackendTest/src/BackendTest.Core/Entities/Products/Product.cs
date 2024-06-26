using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendTest.Entities.Products
{
    public class Product : AuditedAggregateRoot<Guid>
    {
        [Required]
        [StringLength(150)]
        public string Name { get; set; }

        [StringLength(150)]
        public string Description { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal UnitPrice { get; set; }
        public decimal Subtotal { get; set; }
    }

}
