using Abp.AspNetCore.Mvc;
using Abp.Application.Services.Dto;
using BackendTest.Controllers;
using BackendTest.Services.Billings.Dto;
using BackendTest.Services.Billings.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BackendTest.Controllers.Products
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BillingController : BackendTestControllerBase
    {
        private readonly IBillingAppService _service;

        public BillingController(IBillingAppService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var billings = await _service.GetAllAsync();
            return Ok(billings);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBillingDto input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var billingDto = await _service.CreateAsync(input);
                return Ok(billingDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao criar faturamento: {ex.Message}");
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateBillingDto input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _service.UpdateAsync(input);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao atualizar faturamento: {ex.Message}");
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
                return StatusCode(500, $"Erro ao excluir faturamento: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> InsereBillingsAsync([FromBody] CreateBillingDto input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var billingDto = await _service.InsereBillingsAsync(input);
                return Ok(billingDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao inserir faturamento e linhas: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CargaBilling()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _service.CargaBillingAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao obter lista de faturamentos: {ex.Message}");
            }
        }
    }
}
