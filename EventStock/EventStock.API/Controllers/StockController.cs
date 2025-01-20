using EventStock.Application.Dto.Stock;
using EventStock.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventStock.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IStockService _stockService;

        public StockController(IStockService stockService)
        {
            _stockService = stockService;
        }

        [HttpPost("create-stock")]
        public async Task<IActionResult> CreateStock([FromBody] CreateStockDto stockDto)
        {
            var result = await _stockService.CreateStockAsync(stockDto);
            if (!result.Succeeded)
            {
                return BadRequest(result.Error);
            }

            return Ok(result.Value);
        }

        [HttpPost("add-user")]
        public async Task<IActionResult> AddUserToStock([FromBody] AddUserToStockDto request)
        {
            var result = await _stockService.AddUserAsync(request.StockId, request.UserId);
            if (!result.Succeeded)
            {
                return BadRequest(result.Error);
            }

            return Ok();
        }

        [HttpGet("get-stock/{id}")]
        public async Task<IActionResult> GetStock(int id)
        {
            var result = await _stockService.GetStockAsync(id);
            if (!result.Succeeded)
            {
                return BadRequest(result.Error);
            }

            return Ok(result.Value);
        }
    }
}
