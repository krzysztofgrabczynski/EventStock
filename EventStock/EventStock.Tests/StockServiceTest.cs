﻿using AutoMapper;
using EventStock.Application.Dto.Stock;
using EventStock.Application.ResultPattern;
using EventStock.Application.Services;
using EventStock.Domain.Models;
using EventStock.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;
using System.Security.Claims;

namespace EventStock.Tests
{
    public class StockServiceTest
    {
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IStockRepository> _stockRepositoryMock;
        private readonly Mock<UserManager<User>> _userManagerMock;
        private readonly Mock<IAuthorizationService> _authorizationServiceMock;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly StockService _stockService;

        public StockServiceTest()
        {
            _mapperMock = new Mock<IMapper>();
            _userManagerMock = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
            _stockRepositoryMock = new Mock<IStockRepository>();
            _authorizationServiceMock = new Mock<IAuthorizationService>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _stockService = new StockService(_mapperMock.Object, _stockRepositoryMock.Object, _userManagerMock.Object, _authorizationServiceMock.Object, _httpContextAccessorMock.Object);
            
            var mockHttpContext = new Mock<HttpContext>();
            var mockUser = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "test@example.com")
            }));

            mockHttpContext.Setup(c => c.User).Returns(mockUser);

            _httpContextAccessorMock
                .Setup(h => h.HttpContext)
                .Returns(mockHttpContext.Object);

            _authorizationServiceMock.Setup(a =>
                a.AuthorizeAsync(
                    It.IsAny<ClaimsPrincipal>(),
                    It.IsAny<object?>(),
                    It.IsAny<string>()
                    ))
                .ReturnsAsync(AuthorizationResult.Success());
        }

        [Fact]
        public async Task CreateStockAsyncPositiveTest()
        {
            // Arrange
            _mapperMock.Setup(m => m.Map<Stock>(It.IsAny<CreateStockDto>())).Returns<CreateStockDto>(s => 
            new Stock()
            {
                Id = 1,
                Name = s.Name,
                Address = s.Address,
                Users = s.Users
            });
            _stockRepositoryMock.Setup(s => s.CreateStockAsync(It.IsAny<Stock>())).ReturnsAsync(1);
            var stock = new CreateStockDto()
            {
                Name = "TestStock",
                Address = null,
                Users = new List<User>()
            };

            // Act
            var result = await _stockService.CreateStockAsync(stock);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Succeeded);
            Assert.Equal(1, result.Value);
        }

        [Fact]
        public async Task CreateStockAsyncNegativeTest()
        {
            // Arrange
            _stockRepositoryMock.Setup(s => s.CreateStockAsync(It.IsAny<Stock>())).ReturnsAsync((int?)null);
            var stock = new CreateStockDto()
            {
                Name = "TestStock",
                Address = null,
                Users = new List<User>()
            };

            // Act
            var result = await _stockService.CreateStockAsync(stock);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.Succeeded);
        }

        [Fact]
        public async Task GetStockAsyncPositiveTest()
        {
            // Arrange
            var user = new User()
            {
                Id = Guid.NewGuid().ToString(),
                Email = "test@email.com",
                FirstName = "TestFirstName",
                LastName = "TestLastName"
            };
            var stock = new Stock()
            {
                Id = 1,
                Name = "TestStock",
                Users = new List<User>() { user }
            };
            _stockRepositoryMock.Setup(s => s.GetStockAsync(stock.Id)).ReturnsAsync(stock);
            _mapperMock.Setup(m => m.Map<ViewStockDto>(It.IsAny<Stock>())).Returns<Stock>(s => new ViewStockDto()
            {
                Name = s.Name,
                Address = s.Address,
                Users = s.Users.Select(u => new UserWithRoleDto()
                {
                    Email = u.Email,
                    FirstName = u.FirstName,
                    LastName = u.LastName
                   
                }).ToList()
            });

            // Act
            var result = await _stockService.GetStockAsync(stock.Id);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Succeeded);
            Assert.Equal(user.Email, result.Value.Users.First().Email);
        }

        [Fact]
        public async Task GetStockAsyncNegatieTest()
        {
            // Arrange
            var user = new User()
            {
                Id = Guid.NewGuid().ToString(),
                Email = "test@email.com",
                FirstName = "TestFirstName",
                LastName = "TestLastName"
            };
            var stock = new Stock()
            {
                Id = 1,
                Name = "TestStock",
                Users = new List<User>() { user }
            };
            _stockRepositoryMock.Setup(s => s.GetStockAsync(stock.Id)).ReturnsAsync((Stock?)null);
            
            // Act
            var result = await _stockService.GetStockAsync(stock.Id);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.Succeeded);
        }

    }
}
