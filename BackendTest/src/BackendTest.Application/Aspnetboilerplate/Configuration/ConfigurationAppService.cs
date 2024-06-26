using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using BackendTest.Configuration.Dto;
using Microsoft.AspNetCore.Mvc;

namespace BackendTest.Configuration
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [AbpAuthorize]
    public class ConfigurationAppService : BackendTestAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}
