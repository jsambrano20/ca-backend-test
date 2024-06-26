using System;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Abp.AspNetCore;
using Abp.AspNetCore.Configuration;
using Abp.AspNetCore.SignalR;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Zero.Configuration;
using BackendTest.Authentication.JwtBearer;
using BackendTest.Configuration;
using BackendTest.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Abp.Dependency;
using BackendTest.Services.Products.Interfaces;
using BackendTest.Services.Products;
using BackendTest.Services.Customers.Interfaces;
using BackendTest.Services.Customers;
using BackendTest.Services.Billings.Interfaces;
using BackendTest.Services.Billings;

namespace BackendTest
{
    [DependsOn(
         typeof(BackendTestApplicationModule),
         typeof(BackendTestEntityFrameworkModule),
         typeof(AbpAspNetCoreModule)
        ,typeof(AbpAspNetCoreSignalRModule)
     )]
    public class BackendTestWebCoreModule : AbpModule
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public BackendTestWebCoreModule(IWebHostEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void PreInitialize()
        {
            Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(
                BackendTestConsts.ConnectionStringName
            );

            // Use database for language management
            Configuration.Modules.Zero().LanguageManagement.EnableDbLocalization();

            //Configuration.Modules.AbpAspNetCore()
            //     .CreateControllersForAppServices(
            //         typeof(BackendTestApplicationModule).GetAssembly()
            //     );

            ConfigureTokenAuth();
        }

        private void ConfigureTokenAuth()
        {
            IocManager.Register<TokenAuthConfiguration>();
            var tokenAuthConfig = IocManager.Resolve<TokenAuthConfiguration>();

            tokenAuthConfig.SecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_appConfiguration["Authentication:JwtBearer:SecurityKey"]));
            tokenAuthConfig.Issuer = _appConfiguration["Authentication:JwtBearer:Issuer"];
            tokenAuthConfig.Audience = _appConfiguration["Authentication:JwtBearer:Audience"];
            tokenAuthConfig.SigningCredentials = new SigningCredentials(tokenAuthConfig.SecurityKey, SecurityAlgorithms.HmacSha256);
            tokenAuthConfig.Expiration = TimeSpan.FromDays(1);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(BackendTestWebCoreModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            // Register application services automatically
            Configuration.Modules.AbpAspNetCore()
                 .CreateControllersForAppServices(
                     typeof(BackendTestApplicationModule).Assembly
                 );

            // Add application parts
            IocManager.Resolve<ApplicationPartManager>()
                .AddApplicationPartsIfNotAddedBefore(typeof(BackendTestWebCoreModule).Assembly);
        }
    }
}
