﻿using System.Threading.Tasks;
using Abp.Application.Services;
using BackendTest.Sessions.Dto;

namespace BackendTest.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
