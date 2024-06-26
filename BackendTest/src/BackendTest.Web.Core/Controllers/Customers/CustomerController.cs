using Abp.AspNetCore.Mvc;
using Abp.Application.Services.Dto;
using BackendTest.Controllers;
using BackendTest.Services.Customers.Interfaces;
using BackendTest.Services.Customers.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BackendTest.Controllers.Products
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CustomerController : BackendTestControllerBase
    {
        private readonly ICustomerAppService _service;

        public CustomerController(ICustomerAppService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var customers = await _service.GetAllAsync();
            return Ok(customers);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCustomerDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _service.CreateAsync(dto);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao criar cliente: {ex.Message}");
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateCustomerDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _service.UpdateAsync(dto);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao atualizar cliente: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _service.DeleteAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao excluir cliente: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CargaCustomer( )
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _service.CargaCustomerAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao obter lista de clientes: {ex.Message}");
            }
        }
    }
}
