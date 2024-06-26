using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendTest.Entities.Customers
{
    public class Customer : AuditedAggregateRoot<Guid>
    {
        [Required]
        [StringLength(150)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(150)]
        public string Address { get; set; }
    }
}
