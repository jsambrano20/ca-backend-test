using AutoMapper;
using BackendTest.Entities.Products;
using BackendTest.Services.Products.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendTest.Mapper
{
    public class ProductAutoMapperProfile : Profile
    {
        public ProductAutoMapperProfile()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<CreateProductDto, Product>().ReverseMap();
            CreateMap<UpdateProductDto, Product>().ReverseMap();
        }
    }
}
