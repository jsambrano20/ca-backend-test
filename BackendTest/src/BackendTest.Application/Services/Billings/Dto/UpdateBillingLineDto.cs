using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendTest.Services.Billings.Dto
{
    public class UpdateBillingLineDto 
    {
        public string Id { get; set; }

        [Required]
        public string ProductId { get; set; }

        [Required]
        [StringLength(200)]
        public string Description { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal UnitPrice { get; set; }
    }
}
