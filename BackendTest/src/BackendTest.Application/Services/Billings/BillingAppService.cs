using Abp.Application.Services.Dto;
using Abp.Application.Services;
using Abp.Domain.Repositories;
using BackendTest.Entities.Billings;
using BackendTest.Entities.Customers;
using BackendTest.Entities.Products;
using BackendTest.Services.Billings.Dto;
using BackendTest.Services.Billings.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using Abp.EntityFrameworkCore.Repositories;

namespace BackendTest.Services.Billings
{
    public class BillingAppService : ApplicationService, IBillingAppService
    {
        private readonly IRepository<Billing, Guid> _billingRepository;
        private readonly IRepository<Customer, Guid> _customerRepository;
        private readonly IRepository<Product, Guid> _productRepository;
        private readonly BillingService _billingService;

        public BillingAppService(
            IRepository<Billing, Guid> billingRepository,
            IRepository<Customer, Guid> customerRepository,
            IRepository<Product, Guid> productRepository,
            BillingService billingService)
        {
            _billingRepository = billingRepository;
            _customerRepository = customerRepository;
            _productRepository = productRepository;
            _billingService = billingService;
        }

        /// <summary>
        /// Carrega os dados de faturamento de uma API externa.
        /// </summary>
        /// <returns>PagedResultDto contendo os dados de faturamento</returns>
        public async Task<PagedResultDto<BillingDto>> CargaBillingAsync()
        {
            // Chama o serviço externo para obter os dados de faturamento
            var httpResponse = await _billingService.GetBillingDataAsync();
            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new UserFriendlyException("Falha ao obter dados de faturamento externos.");
            }

            // Deserializa a resposta da API externa
            var billingDataString = await httpResponse.Content.ReadAsStringAsync();
            var externalBillings = JsonConvert.DeserializeObject<List<BillingDto>>(billingDataString);

            var insertedBillings = new List<Billing>();

            // Obtém os nomes dos clientes das faturas externas
            var customerNames = externalBillings.Where(b => b.Customer != null).Select(b => b.Customer.Name).ToList();
            // Verifica se os clientes já existem no banco de dados
            var existingCustomers = await _customerRepository.GetAll().Where(c => customerNames.Contains(c.Name)).ToListAsync();

            foreach (var billingDto in externalBillings)
            {
                // Ignora faturas com cliente nulo
                if (billingDto.Customer == null || string.IsNullOrEmpty(billingDto.Customer.Name))
                {
                    Logger.Warn("Ignorando faturamento com cliente nulo.");
                    continue;
                }

                // Verifica se o cliente existe
                var customerExists = existingCustomers.Any(c => c.Name == billingDto.Customer.Name);
                var allProductsExist = true;

                if (billingDto.Lines != null)
                {
                    // Verifica se todos os produtos nas linhas da fatura existem
                    foreach (var line in billingDto.Lines)
                    {
                        var productExists = await _productRepository.GetAll().AnyAsync(p => p.Description == line.Description);
                        if (!productExists)
                        {
                            allProductsExist = false;
                            break;
                        }
                    }
                }

                if (customerExists && allProductsExist)
                {
                    // Mapeia o DTO para a entidade e adiciona à lista de inserções
                    var billing = ObjectMapper.Map<Billing>(billingDto);
                    insertedBillings.Add(billing);
                }
                else
                {
                    // Caso onde apenas o cliente ou apenas os produtos existem
                    if (!customerExists)
                    {
                        Logger.Error("Ocorreu um erro ao atualizar o faturamento. Clientes não existem.");
                        throw new UserFriendlyException("Clientes não existem.");
                    }
                    else if (!allProductsExist)
                    {
                        var nonExistingProducts = billingDto.Lines.Where(line => !_productRepository.GetAll().Any(p => p.Description == line.Description)).Select(line => line.Description);
                        Logger.Error("Ocorreu um erro ao atualizar o faturamento. Produtos não existem.");
                        throw new UserFriendlyException($"Produtos não existem: {string.Join(", ", nonExistingProducts)}");
                    }
                }
            }

            // Insere as faturas no repositório
            await _billingRepository.InsertRangeAsync(insertedBillings);

            // Mapeia as faturas inseridas de volta para DTOs
            var insertedBillingDtos = ObjectMapper.Map<List<BillingDto>>(insertedBillings);
            return new PagedResultDto<BillingDto>(totalCount: insertedBillingDtos.Count, items: insertedBillingDtos);
        }

        /// <summary>
        /// Cria uma nova fatura com base nos dados da API externa.
        /// </summary>
        /// <param name="input">Dados de criação da fatura</param>
        /// <returns>DTO da fatura criada</returns>
        public async Task<BillingDto> CreateAsync(CreateBillingDto input)
        {
            try
            {
                // Obtém dados da API externa
                var httpResponse = await _billingService.GetBillingDataAsync();
                if (!httpResponse.IsSuccessStatusCode)
                {
                    throw new UserFriendlyException("Falha ao obter dados de faturamento externos.");
                }

                // Deserializa a resposta da API externa
                var billingDataString = await httpResponse.Content.ReadAsStringAsync();
                var externalBillings = JsonConvert.DeserializeObject<List<BillingDto>>(billingDataString);

                // Encontra a fatura correspondente pelo número da fatura
                var matchingBilling = externalBillings.FirstOrDefault(eb => eb.InvoiceNumber == input.InvoiceNumber);
                if (matchingBilling == null)
                {
                    throw new UserFriendlyException("Nenhum registro de faturamento externo correspondente encontrado.");
                }

                // Verifica se o cliente existe
                var customerExists = await _customerRepository.GetAll().AnyAsync(c => c.Id.ToString() == matchingBilling.Customer.Id);
                if (!customerExists)
                {
                    throw new UserFriendlyException("Cliente especificado não existe.");
                }

                // Verifica se os produtos existem
                foreach (var line in matchingBilling.Lines)
                {
                    var productExists = await _productRepository.GetAll().AnyAsync(p => p.Id.ToString() == line.ProductId);
                    if (!productExists)
                    {
                        throw new UserFriendlyException($"Produto com ID {line.ProductId} não existe.");
                    }
                }

                // Mapeia o DTO para a entidade e insere no repositório
                var billing = ObjectMapper.Map<Billing>(input);
                await _billingRepository.InsertAsync(billing);

                return ObjectMapper.Map<BillingDto>(billing);
            }
            catch (Exception ex)
            {
                Logger.Error("Ocorreu um erro ao criar o faturamento", ex);
                throw new UserFriendlyException("Houve um erro ao criar o faturamento. Por favor, tente novamente mais tarde.");
            }
        }

        /// <summary>
        /// Insere uma nova fatura e suas linhas.
        /// </summary>
        /// <param name="input">Dados de criação da fatura</param>
        /// <returns>DTO da fatura criada</returns>
        public async Task<BillingDto> InsereBillingsAsync(CreateBillingDto input)
        {
            // Mapeia o DTO para a entidade
            var billing = ObjectMapper.Map<Billing>(input);
            // Insere a fatura no repositório
            var insertedBilling = await _billingRepository.InsertAsync(billing);

            return ObjectMapper.Map<BillingDto>(insertedBilling);
        }

        /// <summary>
        /// Obtém todas as faturas.
        /// </summary>
        /// <returns>Lista de DTOs de faturas</returns>
        public async Task<IEnumerable<BillingDto>> GetAllAsync()
        {
            var billings = await _billingRepository.GetAllListAsync();
            return ObjectMapper.Map<List<BillingDto>>(billings);
        }

        /// <summary>
        /// Atualiza uma fatura existente.
        /// </summary>
        /// <param name="dto">Dados de atualização da fatura</param>
        public async Task UpdateAsync(UpdateBillingDto dto)
        {
            try
            {
                var billing = ObjectMapper.Map<Billing>(dto);
                await _billingRepository.UpdateAsync(billing);
            }
            catch (Exception ex)
            {
                Logger.Error("Ocorreu um erro ao atualizar o faturamento", ex);
                throw new UserFriendlyException("Houve um erro ao atualizar o faturamento. Por favor, tente novamente mais tarde.");
            }
        }

        /// <summary>
        /// Deleta uma fatura pelo ID.
        /// </summary>
        /// <param name="id">ID da fatura</param>
        public async Task DeleteAsync(Guid id)
        {
            var billing = await _billingRepository.GetAsync(id);
            await _billingRepository.DeleteAsync(billing);
        }
    }
}
