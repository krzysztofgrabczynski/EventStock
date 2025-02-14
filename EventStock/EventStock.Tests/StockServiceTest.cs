using AutoMapper;
using EventStock.Application.Dto.Stock;
using EventStock.Application.Dto.User;
using EventStock.Application.ResultPattern;
using EventStock.Application.Services;
using EventStock.Domain.Interfaces;
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
        private readonly Mock<IRoleRepository> _roleRepositoryMock; 
        private readonly Mock<UserManager<User>> _userManagerMock;
        private readonly Mock<IAuthorizationService> _authorizationServiceMock;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly StockService _stockService;

        public StockServiceTest()
        {
            _mapperMock = new Mock<IMapper>();
            _userManagerMock = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
            _stockRepositoryMock = new Mock<IStockRepository>();
            _roleRepositoryMock = new Mock<IRoleRepository>();
            _authorizationServiceMock = new Mock<IAuthorizationService>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _stockService = new StockService(_mapperMock.Object, _stockRepositoryMock.Object, _userManagerMock.Object, _roleRepositoryMock.Object, _authorizationServiceMock.Object, _httpContextAccessorMock.Object);
            
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

        //[Fact]
        //public async Task CreateStockAsyncPositiveTest()
        //{
        //    // Arrange
        //    _mapperMock.Setup(m => m.Map<Stock>(It.IsAny<CreateStockDto>())).Returns<CreateStockDto>(s => 
        //    new Stock()
        //    {
        //        Id = 1,
        //        Name = s.Name,
        //        Address = s.Address,
        //        Users = s.Users
        //    });
        //    _stockRepositoryMock.Setup(s => s.CreateStockAsync(It.IsAny<Stock>())).ReturnsAsync(1);
        //    var stock = new CreateStockDto()
        //    {
        //        Name = "TestStock",
        //        Address = null,
        //        Users = new List<User>()
        //    };

        //    // Act
        //    var result = await _stockService.CreateStockAsync(stock);

        //    // Assert
        //    Assert.NotNull(result);
        //    Assert.True(result.Succeeded);
        //    Assert.Equal(1, result.Value);
        //}

        //[Fact]
        //public async Task CreateStockAsyncNegativeTest()
        //{
        //    // Arrange
        //    _stockRepositoryMock.Setup(s => s.CreateStockAsync(It.IsAny<Stock>())).ReturnsAsync((int?)null);
        //    var stock = new CreateStockDto()
        //    {
        //        Name = "TestStock",
        //        Address = null,
        //        Users = new List<User>()
        //    };

        //    // Act
        //    var result = await _stockService.CreateStockAsync(stock);

        //    // Assert
        //    Assert.NotNull(result);
        //    Assert.False(result.Succeeded);
        //}

        //[Fact]
        //public async Task GetStockAsyncPositiveTest()
        //{
        //    // Arrange
        //    var user = new User()
        //    {
        //        Id = Guid.NewGuid().ToString(),
        //        Email = "test@email.com",
        //        FirstName = "TestFirstName",
        //        LastName = "TestLastName"
        //    };
        //    var stock = new Stock()
        //    {
        //        Id = 1,
        //        Name = "TestStock",
        //        Users = new List<User>() { user }
        //    };
        //    _stockRepositoryMock.Setup(s => s.GetStockAsync(stock.Id)).ReturnsAsync(stock);
        //    _mapperMock.Setup(m => m.Map<ViewStockDto>(It.IsAny<Stock>())).Returns<Stock>(s => new ViewStockDto()
        //    {
        //        Name = s.Name,
        //        Address = s.Address,
        //        Users = s.Users.Select(u => new UserWithRoleDto()
        //        {
        //            Email = u.Email,
        //            FirstName = u.FirstName,
        //            LastName = u.LastName
                   
        //        }).ToList()
        //    });

        //    // Act
        //    var result = await _stockService.GetStockAsync(stock.Id);

        //    // Assert
        //    Assert.NotNull(result);
        //    Assert.True(result.Succeeded);
        //    Assert.Equal(user.Email, result.Value.Users.First().Email);
        //}

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

        [Fact]
        public async Task DeleteUserAsyncWithNotExistingUserInStockTest()
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
                Users = new List<User>()
            };
            _stockRepositoryMock.Setup(s => s.GetStockAsync(stock.Id)).ReturnsAsync(stock);
            _userManagerMock.Setup(u => u.FindByIdAsync(user.Id)).ReturnsAsync(user);

            // Act
            var result = await _stockService.DeleteUserAsync(stock.Id, user.Id);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.Succeeded);
            Assert.Equal("UserNotExistInStock", result.Error.Code);
        }

        [Fact]
        public async Task ListUsersByStockIdAsyncPositiveTest()
        {
            // Arrange
            var user1 = new User()
            {
                Id = Guid.NewGuid().ToString(),
                Email = "test1@email.com",
                FirstName = "TestFirstName1",
                LastName = "TestLastName1"
            };
            var user2 = new User()
            {
                Id = Guid.NewGuid().ToString(),
                Email = "test2@email.com",
                FirstName = "TestFirstName2",
                LastName = "TestLastName2"
            };
            var stock = new Stock()
            {
                Id = 1,
                Name = "TestStock",
                Users = new List<User>() { user1, user2 }
            };
            _stockRepositoryMock.Setup(s => s.GetStockAsync(stock.Id)).ReturnsAsync(stock);
            _mapperMock.Setup(m => m.Map<UserDto>(It.IsAny<User>())).Returns<User>(u =>
                new UserDto()
                {
                    Email = u.Email,
                    FirstName = u.FirstName,
                    LastName = u.LastName
                });

            // Act
            var result = await _stockService.ListUsersByStockIdAsync(stock.Id);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Succeeded);
            Assert.Equal(result.Value.First().Email, user1.Email);
            Assert.Equal(result.Value.Last().Email, user2.Email);
        }

        [Fact]
        public async Task AddUserAsyncPositiveTest()
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
                Users = new List<User>()
            };
            var role = Role.StockAdmin;
            var identityRole = new IdentityRole()
            {
                Name = "StockAdmin",
                NormalizedName = "StockAdmin"
            };
            _stockRepositoryMock.Setup(s => s.GetStockAsync(stock.Id)).ReturnsAsync(stock);
            _userManagerMock.Setup(u => u.FindByIdAsync(user.Id)).ReturnsAsync(user);
            _roleRepositoryMock.Setup(r => r.GetRoleByNameAsync(role.ToString())).ReturnsAsync(identityRole);
            _stockRepositoryMock.Setup(s => s.AddUserAsync(stock, user, identityRole)).ReturnsAsync(true);

            // Act
            var result = await _stockService.AddUserAsync(stock.Id, user.Id, role.ToString());

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Succeeded);
        }

        [Fact]
        public async Task AddUserAsyncWithUserAlreadyAssignedToStockTest()
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
            var role = Role.StockAdmin;
            var identityRole = new IdentityRole()
            {
                Name = "StockAdmin",
                NormalizedName = "StockAdmin"
            };
            _stockRepositoryMock.Setup(s => s.GetStockAsync(stock.Id)).ReturnsAsync(stock);
            _userManagerMock.Setup(u => u.FindByIdAsync(user.Id)).ReturnsAsync(user);

            // Act
            var result = await _stockService.AddUserAsync(stock.Id, user.Id, role.ToString());

            // Assert
            Assert.NotNull(result);
            Assert.False(result.Succeeded);
            Assert.Equal("UserExistInStock", result.Error.Code);
        }

        [Fact]
        public async Task AddUserAsyncWithInvalidRoleTest()
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
                Users = new List<User>()
            };
            var role = "InvalidRole";
            var identityRole = new IdentityRole()
            {
                Name = "StockAdmin",
                NormalizedName = "StockAdmin"
            };
            _stockRepositoryMock.Setup(s => s.GetStockAsync(stock.Id)).ReturnsAsync(stock);
            _userManagerMock.Setup(u => u.FindByIdAsync(user.Id)).ReturnsAsync(user);
            _roleRepositoryMock.Setup(r => r.GetRoleByNameAsync(role.ToString())).ReturnsAsync((IdentityRole?)null);

            // Act
            var result = await _stockService.AddUserAsync(stock.Id, user.Id, role.ToString());

            // Assert
            Assert.NotNull(result);
            Assert.False(result.Succeeded);
            Assert.Equal("RoleDoesNotExist", result.Error.Code);
        }

        [Fact]
        public async Task UpdateUserRoleAsyncPositiveTest()
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
            var role = Role.StockUser;
            var newIdentityRole = new IdentityRole()
            {
                Name = "StockAdmin",
                NormalizedName = "StockAdmin"
            };
            _stockRepositoryMock.Setup(s => s.GetStockAsync(stock.Id)).ReturnsAsync(stock);
            _userManagerMock.Setup(u => u.FindByIdAsync(user.Id)).ReturnsAsync(user);
            _roleRepositoryMock.Setup(r => r.GetRoleByNameAsync(role.ToString())).ReturnsAsync(newIdentityRole);
            _stockRepositoryMock.Setup(s => s.UpdateUserRoleAsync(stock, user, newIdentityRole)).ReturnsAsync(true);

            // Act
            var result = await _stockService.UpdateUserRoleAsync(stock.Id, user.Id, role.ToString());

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Succeeded);
        }

        [Fact]
        public async Task UpdateUserRoleAsyncWithUserNotAssignedToStockTest()
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
                Users = new List<User>()
            };
            var role = Role.StockUser;
            var newIdentityRole = new IdentityRole()
            {
                Name = "StockAdmin",
                NormalizedName = "StockAdmin"
            };
            _stockRepositoryMock.Setup(s => s.GetStockAsync(stock.Id)).ReturnsAsync(stock);
            _userManagerMock.Setup(u => u.FindByIdAsync(user.Id)).ReturnsAsync(user);

            // Act
            var result = await _stockService.UpdateUserRoleAsync(stock.Id, user.Id, role.ToString());

            // Assert
            Assert.NotNull(result);
            Assert.False(result.Succeeded);
            Assert.Equal("UserNotExistInStock", result.Error.Code);
        }
    }
}
