using AutoMapper;
using BackendTest.Entities.Billings;
using BackendTest.Entities.Products;
using BackendTest.Services.Billings.Dto;
using BackendTest.Services.Products.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendTest.Mapper
{
    public class BillingAutoMapperProfile : Profile
    {
        public BillingAutoMapperProfile()
        {
            CreateMap<Billing, BillingDto>().ReverseMap();
            CreateMap<CreateBillingDto, Billing>().ReverseMap();
            CreateMap<UpdateBillingDto, Billing>().ReverseMap();


            CreateMap<BillingLine, BillingLineDto>().ReverseMap();
            CreateMap<CreateBillingLineDto, BillingLine>().ReverseMap();
            CreateMap<UpdateBillingLineDto, BillingLine>().ReverseMap();
        }
    }
}
