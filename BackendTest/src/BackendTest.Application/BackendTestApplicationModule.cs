using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using BackendTest.Authorization;

namespace BackendTest
{
    [DependsOn(
        typeof(BackendTestCoreModule), 
        typeof(AbpAutoMapperModule))]
    public class BackendTestApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<BackendTestAuthorizationProvider>();
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(BackendTestApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddMaps(thisAssembly)
            );
        }
    }
}
