using AutoMapper;
using EventStock.Application.Dto.Equipment;
using EventStock.Application.Dto.Event;
using EventStock.Application.Dto.Stock;
using EventStock.Application.Dto.User;
using EventStock.Application.Interfaces;
using EventStock.Application.ResultPattern;
using EventStock.Application.ResultPattern.Errors;
using EventStock.Domain.Interfaces;
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
        private readonly IRoleRepository _roleRepository;
        private readonly IAuthorizationService _authorizationService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public StockService(IMapper mapper, IStockRepository stockRepository, UserManager<User> userManager, IRoleRepository roleRepository, IAuthorizationService authorizationService, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _stockRepository = stockRepository;
            _userManager = userManager;
            _roleRepository = roleRepository;
            _authorizationService = authorizationService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result> AddUserAsync(int stockId, string userId, string roleName)
        {
            var stock = await _stockRepository.GetStockAsync(stockId);
            var user = await _userManager.FindByIdAsync(userId);

            var authorizationResult = await _authorizationService.AuthorizeAsync(_httpContextAccessor.HttpContext.User, stock, "IsStockUser");
            if (!authorizationResult.Succeeded)
            {
                return Result.Failure(new PermissionDeniedResultError());
            }

            if (stock == null || user == null)
            {
                return Result.Failure(new StockAddUserResultError());
            }

            if (stock.Users.Any(u => u.Id == userId))
            {
                return Result.Failure(new UserExistInStockResultError());
            }

            var role = await _roleRepository.GetRoleByNameAsync(roleName);
            if (role == null)
            {
                return Result.Failure(new RoleDoesNotExistResultError());
            }

            if (!await _stockRepository.AddUserAsync(stock, user, role))
            {
                return Result.Failure(new StockAddUserResultError());
            }

            return Result.Success();
        }

        public async Task<Result> UpdateUserRoleAsync(int stockId, string userId, string roleName)
        {
            var stock = await _stockRepository.GetStockAsync(stockId);
            var user = await _userManager.FindByIdAsync(userId);

            var authorizationResult = await _authorizationService.AuthorizeAsync(_httpContextAccessor.HttpContext.User, stock, "IsStockUser");
            if (!authorizationResult.Succeeded)
            {
                return Result.Failure(new PermissionDeniedResultError());
            }

            if (stock == null || user == null)
            {
                return Result.Failure(new StockAddUserResultError());
            }

            if (!stock.Users.Any(u => u.Id == userId))
            {
                return Result.Failure(new UserNotExistInStockResultError());
            }

            var role = await _roleRepository.GetRoleByNameAsync(roleName);
            if (role == null)
            {
                return Result.Failure(new RoleDoesNotExistResultError());
            }

            if (!await _stockRepository.UpdateUserRoleAsync(stock, user, role))
            {
                return Result.Failure(new UpdateUserRoleResultError());
            }
            
            return Result.Success();
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

            return Result<ViewStockDto>.Success(mappedStock);
        }

        public async Task<Result<List<UserDto>>> ListUsersByStockIdAsync(int stockId)
        {
            var stock = await _stockRepository.GetStockAsync(stockId);
            if (stock == null)
            {
                return Result<List<UserDto>>.Failure(new StockDoesNotExistResultError());
            }

            var authorizationResult = await _authorizationService.AuthorizeAsync(_httpContextAccessor.HttpContext.User, stock, "IsStockUser");
            if (!authorizationResult.Succeeded)
            {
                return Result<List<UserDto>>.Failure(new PermissionDeniedResultError());
            }

            var users = stock.Users.ToList();

            var mappedUsers = new List<UserDto>();
            foreach (var user in users)
            {
                var mappedUser = _mapper.Map<UserDto>(user);
                mappedUsers.Add(mappedUser);
            }

            return Result<List<UserDto>>.Success(mappedUsers);
        }

        public async Task<Result> UpdateStockAsync(int stockId, ViewStockDtoForList stockDto)
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

            stock.Name = stockDto.Name ?? stock.Name;
            stock.Address = stockDto.Address ?? stockDto.Address;

            await _stockRepository.UpdateStockAsync(stock);

            return Result.Success();
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
    }
}
