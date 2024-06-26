using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendTest.Services.Billings.Dto
{
    public class UpdateBillingDto
    {
        public string Id { get; set; }

        [Required]
        [StringLength(50)]
        public string InvoiceNumber { get; set; }

        [Required]
        public string CustomerId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        [Required]
        [StringLength(3)]
        public string Currency { get; set; }

        public List<UpdateBillingLineDto> Lines { get; set; } = new List<UpdateBillingLineDto>();
    }
}
