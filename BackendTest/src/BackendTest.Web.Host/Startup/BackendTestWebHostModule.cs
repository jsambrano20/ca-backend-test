using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using BackendTest.Configuration;

namespace BackendTest.Web.Host.Startup
{
    [DependsOn(
       typeof(BackendTestWebCoreModule))]
    public class BackendTestWebHostModule: AbpModule
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public BackendTestWebHostModule(IWebHostEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(BackendTestWebHostModule).GetAssembly());
        }
    }
}
