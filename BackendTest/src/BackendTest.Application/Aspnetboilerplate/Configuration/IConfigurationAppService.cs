using System.Threading.Tasks;
using BackendTest.Configuration.Dto;

namespace BackendTest.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
