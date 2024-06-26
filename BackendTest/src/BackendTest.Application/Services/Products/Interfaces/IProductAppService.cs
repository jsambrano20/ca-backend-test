using Abp.Application.Services.Dto;
using Abp.Application.Services;
using BackendTest.Services.Billings.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackendTest.Services.Products.Dto;

namespace BackendTest.Services.Products.Interfaces
{
    public interface IProductAppService
    {
        Task<IEnumerable<ProductDto>> GetAllAsync();

        Task CreateAsync(CreateProductDto dto);

        Task UpdateAsync(UpdateProductDto dto);

        Task DeleteAsync(Guid id);

        Task<PagedResultDto<ProductDto>> CargaProductAsync();

    }
}
