using EventStock.Application.Dto.Stock;
using EventStock.Application.Interfaces;
using EventStock.Domain.Models;
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

        [HttpPut("update-stock/{id}")]
        [Authorize(Policy = "StockModeratorPolicy")]
        public async Task<IActionResult> UpdateStock(int id, [FromBody] ViewStockDtoForList stockDto)
        {
            var result = await _stockService.UpdateStockAsync(id, stockDto);
            if (!result.Succeeded)
            {
                return BadRequest(result.Error);
            }

            return Ok();
        }


        [HttpDelete("delete-stock/{id}")]
        [Authorize(Policy = "StockAdminPolicy")]
        public async Task<IActionResult> DeleteStock(int id)
        {
            var result = await _stockService.DeleteStockAsync(id);
            if (!result.Succeeded)
            {
                return BadRequest(result.Error);
            }

            return Ok();
        }

        [HttpPost("add-user")]
        [Authorize(Policy = "StockModeratorPolicy")]
        public async Task<IActionResult> AddUserToStock([FromBody] AddUserToStockDto request)
        {
            if (!Enum.IsDefined(typeof(Role), request.Role))
            {
                return BadRequest("Provided role does not exist");
            }

            var result = await _stockService.AddUserAsync(request.StockId, request.UserId, request.Role.ToString());
            if (!result.Succeeded)
            {
                return BadRequest(result.Error);
            }

            return Ok();
        }

        [HttpPut("update-user-role")]
        [Authorize(Policy = "StockModeratorPolicy")]
        public async Task<IActionResult> UpdateUserRoleInStock([FromBody] AddUserToStockDto request)
        {
            if (!Enum.IsDefined(typeof(Role), request.Role))
            {
                return BadRequest("Provided role does not exist");
            }

            var result = await _stockService.UpdateUserRoleAsync(request.StockId, request.UserId, request.Role.ToString());
            if (!result.Succeeded)
            {
                return BadRequest(result.Error);
            }

            return Ok();
        }

        [HttpDelete("delete-user")]
        [Authorize(Policy = "StockModeratorPolicy")]
        public async Task<IActionResult> DeleteUserFromStock([FromBody] DeleteUserFromStockDto request)
        {
            var result = await _stockService.DeleteUserAsync(request.StockId, request.UserId);
            if (!result.Succeeded)
            {
                return BadRequest(result.Error);
            }

            return Ok();
        }

        [HttpGet("list-stock-users/{id}")]
        public async Task<IActionResult> ListUsersInStock(int id)
        {
            var result = await _stockService.ListUsersByStockIdAsync(id);
            if (!result.Succeeded)
            {
                return BadRequest(result.Error);
            }

            return Ok(result.Value);
        }
    }
}
