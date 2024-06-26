using Abp.Application.Services.Dto;
using Abp.Application.Services;
using Abp.Domain.Repositories;
using BackendTest.Entities.Products;
using BackendTest.Services.Products.Dto;
using BackendTest.Services.Billings;
using BackendTest.Services.Billings.Dto;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.UI;
using BackendTest.Services.Products.Interfaces;
using Abp.EntityFrameworkCore.Repositories;

namespace BackendTest.Services.Products
{
    public class ProductAppService : ApplicationService, IProductAppService
    {
        private readonly IRepository<Product, Guid> _repository;
        private readonly BillingService _billingService;

        public ProductAppService(
            IRepository<Product, Guid> repository,
            BillingService billingService)
        {
            _repository = repository;
            _billingService = billingService;
        }

        /// <summary>
        /// Carga inicial de produtos a partir dos dados de faturamento externos.
        /// </summary>
        /// <returns>PagedResultDto contendo os produtos</returns>
        public async Task<PagedResultDto<ProductDto>> CargaProductAsync()
        {
            try
            {
                // Obtém todos os produtos do banco de dados local
                var products = await _repository.GetAllListAsync();

                // Obtém os dados de faturamento externos usando o serviço BillingService
                var billingDataResponse = await _billingService.GetBillingDataAsync();

                // Verifica se a requisição para obter os dados de faturamento foi bem-sucedida
                if (billingDataResponse.IsSuccessStatusCode)
                {
                    // Lê os dados de resposta como uma string
                    var billingDataString = await billingDataResponse.Content.ReadAsStringAsync();

                    // Converte os dados de faturamento de JSON para uma lista de objetos BillingDto
                    var billingsJsonData = JsonConvert.DeserializeObject<List<BillingDto>>(billingDataString);

                    // Lista para armazenar novos produtos a serem adicionados
                    var productsToAdd = new List<Product>();

                    // Itera sobre cada faturamento recebido
                    foreach (var billing in billingsJsonData)
                    {
                        // Verifica se há linhas de produtos no faturamento
                        if (billing.Lines != null)
                        {
                            // Itera sobre cada linha de produto no faturamento
                            foreach (var billingProduct in billing.Lines)
                            {
                                // Verifica se já existe um produto com a mesma descrição no banco de dados local
                                var existingProduct = products.FirstOrDefault(p => p.Description == billingProduct.Description);

                                // Se não existir produto com a mesma descrição, cria um novo produto localmente
                                if (existingProduct == null)
                                {
                                    // Cria um novo objeto de produto com os dados da linha de faturamento
                                    var newProduct = new Product
                                    {
                                        Name = billingProduct.Description,
                                        Description = billingProduct.Description,
                                        UnitPrice = billingProduct.UnitPrice,
                                        Subtotal = billingProduct.Subtotal
                                    };

                                    // Adiciona o novo produto à lista de produtos a serem adicionados
                                    productsToAdd.Add(newProduct);
                                }
                            }
                        }
                    }

                    // Insere os novos produtos na base de dados local
                    if (productsToAdd.Any())
                    {
                        await _repository.InsertRangeAsync(productsToAdd);
                    }

                    // Atualiza a lista completa de produtos após a inserção dos novos produtos
                    products = await _repository.GetAllListAsync();
                }
                else
                {
                    // Lança uma exceção se não conseguir obter os dados de faturamento externos
                    throw new Exception("Não foi possível recuperar dados de faturamento.");
                }

                // Converte a lista de produtos do banco de dados para uma lista de ProductDto utilizando o ObjectMapper
                var productDtos = ObjectMapper.Map<List<ProductDto>>(products);

                // Retorna um PagedResultDto contendo os produtos mapeados e o total de produtos
                return new PagedResultDto<ProductDto>(products.Count, productDtos);
            }
            catch (Exception ex)
            {
                // Registra um erro no log se ocorrer um erro ao obter ou inserir os produtos
                Logger.Error("Ocorreu um erro ao obter ou inserir os produtos", ex);

                // Lança uma exceção amigável se ocorrer um erro ao obter ou inserir os produtos
                throw new UserFriendlyException("Houve um erro ao obter ou inserir os produtos. Por favor, tente novamente mais tarde.");
            }
        }


        /// <summary>
        /// Obtém todos os produtos.
        /// </summary>
        /// <returns>Lista de ProductDto contendo todos os produtos</returns>
        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            // Obtém todos os produtos do banco de dados local
            var products = await _repository.GetAllListAsync();

            // Converte a lista de produtos do banco de dados para uma lista de ProductDto utilizando o ObjectMapper
            return ObjectMapper.Map<List<ProductDto>>(products);
        }

        /// <summary>
        /// Cria um novo produto.
        /// </summary>
        /// <param name="dto">Dados do produto a ser criado</param>
        public async Task CreateAsync(CreateProductDto dto)
        {
            try
            {
                // Verifica se já existe um produto com a mesma descrição no banco de dados local
                var existingProduct = await _repository.FirstOrDefaultAsync(p => p.Description == dto.Description);

                // Se já existir um produto com a mesma descrição, lança uma exceção amigável
                if (existingProduct != null)
                {
                    throw new UserFriendlyException($"Produto '{dto.Description}' já existe.");
                }

                // Converte os dados do DTO para o objeto de entidade Product e insere no banco de dados local
                var product = ObjectMapper.Map<Product>(dto);
                await _repository.InsertAsync(product);
            }
            catch (Exception ex)
            {
                // Registra um erro no log se ocorrer um erro ao criar o produto
                Logger.Error("Ocorreu um erro ao criar o produto", ex);

                // Lança uma exceção amigável se ocorrer um erro ao criar o produto
                throw new UserFriendlyException("Houve um erro ao criar o produto. Por favor, tente novamente mais tarde.");
            }
        }

        /// <summary>
        /// Atualiza um produto existente.
        /// </summary>
        /// <param name="dto">Dados do produto a ser atualizado</param>
        public async Task UpdateAsync(UpdateProductDto dto)
        {
            try
            {
                // Converte os dados do DTO para o objeto de entidade Product e atualiza no banco de dados local
                var product = ObjectMapper.Map<Product>(dto);
                await _repository.UpdateAsync(product);
            }
            catch (Exception ex)
            {
                // Registra um erro no log se ocorrer um erro ao atualizar o produto
                Logger.Error("Ocorreu um erro ao atualizar o produto", ex);

                // Lança uma exceção amigável se ocorrer um erro ao atualizar o produto
                throw new UserFriendlyException("Houve um erro ao atualizar o produto. Por favor, tente novamente mais tarde.");
            }
        }

        /// <summary>
        /// Deleta um produto existente pelo ID.
        /// </summary>
        /// <param name="id">ID do produto a ser deletado</param>
        public async Task DeleteAsync(Guid id)
        {
            // Obtém o produto pelo ID no banco de dados local e deleta
            var product = await _repository.GetAsync(id);
            await _repository.DeleteAsync(product);
        }
    }
}
