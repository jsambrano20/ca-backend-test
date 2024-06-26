using AutoMapper;
using BackendTest.Entities.Customers;
using BackendTest.Entities.Products;
using BackendTest.Services.Customers.Dto;
using BackendTest.Services.Products.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendTest.Mapper
{
    public class CustomerAutoMapperProfile : Profile
    {
        public CustomerAutoMapperProfile()
        {
            CreateMap<Customer, CustomerDto>().ReverseMap();
            CreateMap<CreateCustomerDto, Customer>().ReverseMap();
            CreateMap<UpdateCustomerDto, Customer>().ReverseMap();
        }
    }
}
