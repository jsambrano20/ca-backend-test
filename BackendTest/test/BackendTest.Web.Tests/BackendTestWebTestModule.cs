using Abp.AspNetCore;
using Abp.AspNetCore.TestBase;
using Abp.Modules;
using Abp.Reflection.Extensions;
using BackendTest.EntityFrameworkCore;
using BackendTest.Web.Startup;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace BackendTest.Web.Tests
{
    [DependsOn(
        typeof(BackendTestWebMvcModule),
        typeof(AbpAspNetCoreTestBaseModule)
    )]
    public class BackendTestWebTestModule : AbpModule
    {
        public BackendTestWebTestModule(BackendTestEntityFrameworkModule abpProjectNameEntityFrameworkModule)
        {
            abpProjectNameEntityFrameworkModule.SkipDbContextRegistration = true;
        } 
        
        public override void PreInitialize()
        {
            Configuration.UnitOfWork.IsTransactional = false; //EF Core InMemory DB does not support transactions.
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(BackendTestWebTestModule).GetAssembly());
        }
        
        public override void PostInitialize()
        {
            IocManager.Resolve<ApplicationPartManager>()
                .AddApplicationPartsIfNotAddedBefore(typeof(BackendTestWebMvcModule).Assembly);
        }
    }
}