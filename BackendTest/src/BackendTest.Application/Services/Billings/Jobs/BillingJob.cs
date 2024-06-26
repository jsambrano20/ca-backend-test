
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendTest.Services.Billings.Jobs
{
    internal class BillingJob
    {
        private readonly IMemoryCache _cache;

        public const string CACHE_CONSULTA = "CACHE_CONSULTA";
        public const int TEMPO_LIMITE_ESPERA_MINUTOS = 30;

        private readonly MemoryCacheEntryOptions CACHE_OPTIONS = new()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(120)
        };

        public BillingJob(IMemoryCache cache)
        {
            _cache = cache;
        }

        //IMPLEMENTAR JOB DIARIO OU A CADA 1 HORA PARA CONSULTA DA API E
        //VERIFICAÇÃO DE NOVOS ITENS DE BILLING/PRODUCT/CUSTOMER


    }
}
