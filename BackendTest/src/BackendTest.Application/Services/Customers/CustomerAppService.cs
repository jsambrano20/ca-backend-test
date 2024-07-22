using Abp.Application.Services.Dto;
using Abp.Application.Services;
using Abp.Domain.Repositories;
using BackendTest.Entities.Customers;
using BackendTest.Services.Customers.Dto;
using BackendTest.Services.Billings; // Importação do serviço de faturamento
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.UI;
using BackendTest.Services.Billings.Dto;
using BackendTest.Services.Customers.Interfaces;
using Abp.EntityFrameworkCore.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BackendTest.Services.Customers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class CustomerAppService : BackendTestAppServiceBase, ICustomerAppService
    {
        private readonly IRepository<Customer, Guid> _repository;
        private readonly BillingService _billingService; // Serviço de faturamento

        public CustomerAppService(IRepository<Customer, Guid> repository, BillingService billingService)
        {
            _repository = repository;
            _billingService = billingService;
        }

        /// <summary>
        /// Carga inicial de clientes a partir dos dados de faturamento externos.
        /// </summary>
        /// <returns>PagedResultDto contendo os clientes</returns>
        public async Task<PagedResultDto<CustomerDto>> CargaCustomerAsync()
        {
            // Obtém os dados de faturamento externos
            var billingDataResponse = await _billingService.GetBillingDataAsync();

            // Verifica se a requisição para obter os dados de faturamento foi bem-sucedida
            if (billingDataResponse.IsSuccessStatusCode)
            {
                // Lê os dados de resposta como uma string
                var billingDataString = await billingDataResponse.Content.ReadAsStringAsync();

                // Converte os dados de faturamento de JSON para uma lista de objetos BillingDto
                var billingsJsonData = JsonConvert.DeserializeObject<List<BillingDto>>(billingDataString);

                // Obtém todos os clientes atuais do banco de dados local
                var customers = await _repository.GetAllListAsync();

                // Lista para armazenar clientes que serão adicionados
                var customersToAdd = new List<Customer>();

                // Itera sobre cada faturamento recebido
                foreach (var billing in billingsJsonData)
                {
                    // Verifica se o objeto de cliente no faturamento não é nulo
                    if (billing.Customer != null)
                    {
                        // Verifica se já existe um cliente com o mesmo nome no banco de dados local
                        var existingCustomer = customers.FirstOrDefault(c => c.Name == billing.Customer.Name);

                        // Se não existir cliente com o mesmo nome, cria um novo cliente localmente
                        if (existingCustomer == null)
                        {
                            // Cria um novo objeto de cliente com os dados do faturamento
                            var customer = new Customer
                            {
                                // Verifica se o nome, email e endereço estão presentes; caso não estejam, utiliza "N/A"
                                Id = Guid.Parse(billing.Customer.Id),
                                Name = billing.Customer.Name ?? "N/A",
                                Email = billing.Customer.Email ?? "N/A",
                                Address = billing.Customer.Address ?? "N/A"
                            };

                            // Verifica se o nome ou o email do cliente são "N/A"; se forem, registra um erro no log e continua para o próximo cliente
                            if (customer.Name == "N/A" || customer.Email == "N/A")
                            {
                                Logger.Error("Nome ou Email do Cliente não consta!");
                                continue;
                            }

                            // Adiciona o novo cliente à lista de clientes a serem adicionados
                            if (!customersToAdd.Exists(x => x.Name.Equals(customer.Name)))
                                customersToAdd.Add(customer);
                        }
                    }
                }

                // Insere os novos clientes na base de dados local
                if (customersToAdd.Any())
                {
                    await _repository.InsertRangeAsync(customersToAdd);
                }

                // Atualiza a lista completa de clientes após a inserção dos novos clientes
                customers = await _repository.GetAllListAsync();

                // Converte a lista de clientes do banco de dados para uma lista de CustomerDto utilizando o ObjectMapper
                var customerDtos = ObjectMapper.Map<List<CustomerDto>>(customers);

                // Retorna um PagedResultDto contendo os clientes mapeados e o total de clientes
                return new PagedResultDto<CustomerDto>(totalCount: customerDtos.Count, items: customerDtos);
            }
            else
            {
                // Lança uma exceção se não conseguir obter os dados de faturamento externos
                throw new Exception("Não foi possível recuperar dados de faturamento.");
            }
        }


        /// <summary>
        /// Obtém todos os clientes.
        /// </summary>
        /// <returns>Lista de DTOs de clientes</returns>
        public async Task<IEnumerable<CustomerDto>> GetAllAsync()
        {
            var customers = await _repository.GetAllListAsync();
            return ObjectMapper.Map<List<CustomerDto>>(customers);
        }

        /// <summary>
        /// Cria um novo cliente.
        /// </summary>
        /// <param name="dto">Dados de criação do cliente</param>
        public async Task CreateAsync(CreateCustomerDto dto)
        {
            try
            {
                var existingCustomer = await _repository.FirstOrDefaultAsync(c => c.Name == dto.Name);
                if (existingCustomer != null)
                {
                    throw new UserFriendlyException($"Cliente '{dto.Name}' já existe.");
                }

                var customer = ObjectMapper.Map<Customer>(dto);
                await _repository.InsertAsync(customer);
            }
            catch (Exception ex)
            {
                Logger.Error("Ocorreu um erro ao criar o cliente", ex);
                throw new UserFriendlyException("Houve um erro ao criar o cliente. Por favor, tente novamente mais tarde.");
            }
        }

        /// <summary>
        /// Atualiza um cliente existente.
        /// </summary>
        /// <param name="dto">Dados de atualização do cliente</param>
        public async Task UpdateAsync(UpdateCustomerDto dto)
        {
            try
            {
                var customer = ObjectMapper.Map<Customer>(dto);
                await _repository.UpdateAsync(customer);
            }
            catch (Exception ex)
            {
                Logger.Error("Ocorreu um erro ao atualizar o cliente", ex);
                throw new UserFriendlyException("Houve um erro ao atualizar o cliente. Por favor, tente novamente mais tarde.");
            }
        }

        /// <summary>
        /// Deleta um cliente pelo ID.
        /// </summary>
        /// <param name="id">ID do cliente</param>
        public async Task DeleteAsync(Guid id)
        {
            var customer = await _repository.GetAsync(id);
            await _repository.DeleteAsync(customer);
        }
    }
}
