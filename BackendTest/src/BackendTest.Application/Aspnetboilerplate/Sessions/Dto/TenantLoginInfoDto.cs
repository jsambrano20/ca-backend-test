using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using BackendTest.MultiTenancy;

namespace BackendTest.Sessions.Dto
{
    [AutoMapFrom(typeof(Tenant))]
    public class TenantLoginInfoDto : EntityDto
    {
        public string TenancyName { get; set; }

        public string Name { get; set; }
    }
}
