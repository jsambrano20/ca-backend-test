using Abp.Application.Services.Dto;
using AutoMapper;
using BackendTest.Entities.Billings;
using BackendTest.Services.Customers.Dto;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendTest.Services.Billings.Dto
{
    public class BillingDto : AuditedEntityDto<string>
    {
        [JsonProperty("invoice_number")] // Anotação para mapeamento do JSON
        public string InvoiceNumber { get; set; }

        [JsonProperty("customer")] // Anotação para mapeamento do 
        public CustomerDto Customer { get; set; }

        public Guid? CustomerId { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("due_date")]
        public DateTime DueDate { get; set; }

        [JsonProperty("total_amount")]
        public decimal TotalAmount { get; set; }
        public string Currency { get; set; }
        public List<BillingLineDto> Lines { get; set; }
    }
}
