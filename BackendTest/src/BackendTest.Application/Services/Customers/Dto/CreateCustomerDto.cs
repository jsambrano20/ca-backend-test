using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendTest.Services.Customers.Dto
{
    public class CreateCustomerDto 
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
