using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.UI;
using Abp.Web.Models;
using BackendTest.Authorization;
using BackendTest.Services.Products.Dto;
using BackendTest.Services.Products.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendTest.Controllers.Products
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductController : BackendTestControllerBase
    {
        private readonly IProductAppService _service;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductAppService service, ILogger<ProductController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                _logger.LogInformation("Starting GetAllAsync method.");
                var products = await _service.GetAllAsync();
                _logger.LogInformation("GetAllAsync method finished successfully.");
                return Ok(new AjaxResponse(products));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while executing GetAllAsync.");
                return StatusCode(500, new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

        [HttpPost]
        public async Task CreateAsync([FromBody] CreateProductDto dto)
        {
            if (dto == null)
            {
                throw new UserFriendlyException("Product data cannot be null.");
            }

            await _service.CreateAsync(dto);
        }

        [HttpPut]
        public async Task UpdateAsync([FromBody] UpdateProductDto dto)
        {
            if (dto == null)
            {
                throw new UserFriendlyException("Product data cannot be null.");
            }

            await _service.UpdateAsync(dto);
        }

        [HttpDelete("{id}")]
        public async Task DeleteAsync(Guid id)
        {
            await _service.DeleteAsync(id);
        }

        [HttpGet]
        public async Task<PagedResultDto<ProductDto>> CargaProduct()
        {
            return await _service.CargaProductAsync();
        }
    }
}
