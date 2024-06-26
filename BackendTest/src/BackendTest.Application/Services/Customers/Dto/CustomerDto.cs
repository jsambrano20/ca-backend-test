using Abp.Application.Services.Dto;
using AutoMapper;
using BackendTest.Entities.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendTest.Services.Customers.Dto
{
    public class CustomerDto : AuditedEntityDto<string>
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
    }
}
