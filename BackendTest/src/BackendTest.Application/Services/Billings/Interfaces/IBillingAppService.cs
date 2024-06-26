using Abp.Application.Services;
using Abp.Application.Services.Dto;
using BackendTest.Services.Billings.Dto;
using BackendTest.Services.Customers.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendTest.Services.Billings.Interfaces
{
    public interface IBillingAppService
    {
        Task<IEnumerable<BillingDto>> GetAllAsync();

        Task<BillingDto> InsereBillingsAsync(CreateBillingDto input);
        Task<BillingDto> CreateAsync(CreateBillingDto input);
        Task UpdateAsync(UpdateBillingDto dto);

        Task DeleteAsync(Guid id);

        Task<PagedResultDto<BillingDto>> CargaBillingAsync();

        Task<byte[]> ExportBillingExcelAsync();
    }
}
