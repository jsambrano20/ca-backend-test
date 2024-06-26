using System.Threading.Tasks;
using Abp.Application.Services;
using BackendTest.Authorization.Accounts.Dto;

namespace BackendTest.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<RegisterOutput> Register(RegisterInput input);
    }
}
