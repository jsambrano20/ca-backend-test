using Abp.Application.Services;
using BackendTest.MultiTenancy.Dto;

namespace BackendTest.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}

