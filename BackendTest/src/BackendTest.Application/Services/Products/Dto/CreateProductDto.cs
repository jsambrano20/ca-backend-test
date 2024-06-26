using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendTest.Services.Products.Dto
{
    public class CreateProductDto
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal Subtotal { get; set; }
    }
}
