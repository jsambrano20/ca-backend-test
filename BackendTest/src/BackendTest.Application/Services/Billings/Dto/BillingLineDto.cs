using Abp.Application.Services.Dto;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendTest.Services.Billings.Dto
{
    public class BillingLineDto
    {
        [JsonProperty("productId")]
        public string ProductId { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }

        [JsonProperty("unit_price")]
        public decimal UnitPrice { get; set; }
        public decimal Subtotal { get; set; }
    }
}
