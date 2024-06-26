using Abp.Application.Services;
using Abp.Application.Services.Dto;
using BackendTest.MultiTenancy.Dto;
using BackendTest.Services.Customers.Dto;
using BackendTest.Services.Products.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendTest.Services.Customers.Interfaces
{
    public interface ICustomerAppService
    {
        Task<IEnumerable<CustomerDto>> GetAllAsync();

        Task CreateAsync(CreateCustomerDto dto);

        Task UpdateAsync(UpdateCustomerDto dto);

        Task DeleteAsync(Guid id);

        Task<PagedResultDto<CustomerDto>> CargaCustomerAsync();
    }

}
