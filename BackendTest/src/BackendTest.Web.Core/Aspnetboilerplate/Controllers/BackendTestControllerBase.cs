using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace BackendTest.Controllers
{
    public abstract class BackendTestControllerBase: AbpController
    {
        protected BackendTestControllerBase()
        {
            LocalizationSourceName = BackendTestConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
