using AutoMapper;
using EventStock.Application.Dto.Equipment;
using EventStock.Application.Dto.Event;
using EventStock.Application.Dto.Stock;
using EventStock.Application.Interfaces;
using EventStock.Application.ResultPattern;
using EventStock.Application.ResultPattern.Errors;
using EventStock.Domain.Models;
using EventStock.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace EventStock.Application.Services
{
    public class StockService : IStockService
    {
        private readonly IMapper _mapper;
        private readonly IStockRepository _stockRepository;
        private readonly UserManager<User> _userManager;
        private readonly IAuthorizationService _authorizationService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public StockService(IMapper mapper, IStockRepository stockRepository, UserManager<User> userManager, IAuthorizationService authorizationService, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _stockRepository = stockRepository;
            _userManager = userManager;
            _authorizationService = authorizationService;
            _httpContextAccessor = httpContextAccessor;
        }

        public Task AddRoleToStockUserAsync(int id, IdentityRole role)
        {
            throw new NotImplementedException();
        }

        public async Task<Result> AddUserAsync(int stockId, string userId)
        {
            var stock = await _stockRepository.GetStockAsync(stockId);
            var user = await _userManager.FindByIdAsync(userId);
            if (stock == null || user == null)
            {
                return Result.Failure(new StockAddUserResultError());
            }

            if (stock.Users.Any(u => u.Id == userId))
            {
                return Result.Failure(new UserExistInStockResultError());
            }
            
            var authorizationResult = await _authorizationService.AuthorizeAsync(_httpContextAccessor.HttpContext.User, stock, "IsStockUser");
            if (!authorizationResult.Succeeded)
            {
                return Result.Failure(new PermissionDeniedResultError());
            }


            var role = new IdentityRole(); // for test
            await _stockRepository.AddUserAsync(stock, user, role);


            return Result.Success();
        }

        public async Task<Result<int>> CreateStockAsync(CreateStockDto stock)
        {
            var mappedStock = _mapper.Map<Stock>(stock);
            var id = await _stockRepository.CreateStockAsync(mappedStock);
            if (id == null)
            {
                return Result<int>.Failure(new StockSavingResultError());
            }
            
            return Result<int>.Success(id.Value); 
        }

        public async Task<Result> DeleteStockAsync(int stockId)
        {
            var stock = await _stockRepository.GetStockAsync(stockId);
            if (stock == null)
            {
                return Result.Failure(new StockDoesNotExistResultError());
            }

            var authorizationResult = await _authorizationService.AuthorizeAsync(_httpContextAccessor.HttpContext.User, stock, "IsStockUser");
            if (!authorizationResult.Succeeded)
            {
                return Result.Failure(new PermissionDeniedResultError());
            }

            await _stockRepository.DeleteStockAsync(stock);
            return Result.Success();
        }

        public async Task<Result> DeleteUserAsync(int stockId, string userId)
        {
            var stock = await _stockRepository.GetStockAsync(stockId);
            var user = await _userManager.FindByIdAsync(userId);
            if (stock == null || user == null)
            {
                return Result.Failure(new StockAddUserResultError());
            }

            if (!stock.Users.Any(u => u.Id == userId))
            {
                return Result.Failure(new UserNotExistInStockResultError());
            }

            var authorizationResult = await _authorizationService.AuthorizeAsync(_httpContextAccessor.HttpContext.User, stock, "IsStockUser");
            if (!authorizationResult.Succeeded)
            {
                return Result.Failure(new PermissionDeniedResultError());
            }

            await _stockRepository.DeleteUserAsync(stock, user);
            return Result.Success();
        }

        public async Task<Result<ViewStockDto>> GetStockAsync(int id)
        {
            var result = await _stockRepository.GetStockAsync(id);
            if (result == null)
            {
                return Result<ViewStockDto>.Failure(new StockDoesNotExistResultError());
            }

            var authorizationResult = await _authorizationService.AuthorizeAsync(_httpContextAccessor.HttpContext.User, result, "IsStockUser");
            if (!authorizationResult.Succeeded)
            {
                return Result<ViewStockDto>.Failure(new PermissionDeniedResultError());
            }

            var mappedStock = _mapper.Map<ViewStockDto>(result);

            foreach (var user in mappedStock.Users)
            {
                var userRole = result.UserStockRoles
                    .Where(u => u.User.Email == user.Email)
                    .Select(u => u.Id)
                    .FirstOrDefault();
                user.Role = userRole;
            }

            return Result<ViewStockDto>.Success(mappedStock);
        }

        public Task<List<ViewEquipmentDto>> ListAvailableStockEquipmentInDateAsync(int id, DateTime date)
        {
            throw new NotImplementedException();
        }

        public Task<List<ViewEquipmentDto>> ListStockEquipmentAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<ViewEventDto>> ListStockEventsAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<ViewEventDto>> ListStockEventsAsync(int id, DateTime date)
        {
            throw new NotImplementedException();
        }

        public Task<List<StockUserDto>> ListUsersByStockIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ViewStockDto> UpdateStockAsync(ViewStockDtoForList stock)
        {
            throw new NotImplementedException();
        }
    }
}
