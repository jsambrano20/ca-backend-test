using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Abp.Dependency;

namespace BackendTest.Services.Billings
{
    public class BillingService : ITransientDependency
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _serviceBillingBaseUrl;

        public BillingService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));

            // Configuração da URL base da API de Billing a partir do appsettings.json
            _serviceBillingBaseUrl = configuration.GetSection("ExternalServices:BillingApi:BaseUrl").Value;
        }

        /// <summary>
        /// Obtém todos os dados de faturamento da API externa.
        /// </summary>
        /// <returns>HttpResponseMessage contendo a resposta da API</returns>
        public async Task<HttpResponseMessage> GetBillingDataAsync()
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient();
                var response = await httpClient.GetAsync($"{_serviceBillingBaseUrl}");
                response.EnsureSuccessStatusCode(); // Lança exceção se a requisição não for bem-sucedida (status code != 2xx)
                return response;
            }
            catch (HttpRequestException ex)
            {
                // Tratamento de exceção caso ocorra um erro na requisição HTTP
                throw new ApplicationException("Erro ao obter dados de faturamento", ex);
            }
        }

        /// <summary>
        /// Obtém os dados das linhas de faturamento de um faturamento específico da API externa.
        /// </summary>
        /// <param name="billingId">ID do faturamento</param>
        /// <returns>HttpResponseMessage contendo a resposta da API</returns>
        public async Task<HttpResponseMessage> GetBillingLinesDataAsync(string billingId)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient();
                var response = await httpClient.GetAsync($"{_serviceBillingBaseUrl}/{billingId}");
                response.EnsureSuccessStatusCode(); // Lança exceção se a requisição não for bem-sucedida (status code != 2xx)
                return response;
            }
            catch (HttpRequestException ex)
            {
                // Tratamento de exceção caso ocorra um erro na requisição HTTP
                throw new ApplicationException($"Erro ao obter dados das linhas de faturamento para o ID {billingId}", ex);
            }
        }
    }
}
