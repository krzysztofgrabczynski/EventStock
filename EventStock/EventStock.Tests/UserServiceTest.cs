using AutoMapper;
using EventStock.Application.Dto.Stock;
using EventStock.Application.Dto.User;
using EventStock.Application.Interfaces;
using EventStock.Application.ResultPattern.Errors;
using EventStock.Application.Services;
using EventStock.Domain.Interfaces;
using EventStock.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace EventStock.Tests
{
    public class UserServiceTest
    {
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<UserManager<User>> _userManagerMock;
        private readonly IUserService _userService;
        private readonly Mock<IUserRepository> _userRepositoryMock;

        public UserServiceTest()
        {
            _mapperMock = new Mock<IMapper>();
            _userManagerMock = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
            _userRepositoryMock = new Mock<IUserRepository>();
            _userService = new UserService(_mapperMock.Object, _userManagerMock.Object, _userRepositoryMock.Object);
        }

        [Fact]
        public async Task CreateUserAsyncTest()
        {
            // Arrange 
            var user = new CreateUserDto()
            {
                Email = "test@email.com",
                FirstName = "John",
                LastName = "Doe",
                Password = "VerySecretPassword123!",
                ConfirmPassword = "VerySecretPassword123!"
            };
            var identityResultSuccess = IdentityResult.Success;

            _userManagerMock.Setup(u => u.CreateAsync(It.IsAny<User>(), user.Password)).ReturnsAsync(identityResultSuccess);

            // Act
            var result = await _userService.CreateUserAsync(user);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Succeeded);
        }

        [Fact]
        public async Task GetUserAsyncPositiveTest()
        {
            // Arrange
            var user = new User()
            {
                Id = "test-id",
                Email = "test@emial.com",
                FirstName = "John",
                LastName = "Doe"
            };
            var userDto = new UserDto()
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
            _userManagerMock.Setup(u => u.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(user);
            _mapperMock.Setup(m => m.Map<UserDto>(user)).Returns(userDto);

            // Act
            var result = await _userService.GetUserAsync("test-id");

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Succeeded);
            Assert.Equal(result.Error, ResultError.None);
            Assert.Equal(result.Value.FirstName, user.FirstName);
        }

        [Fact]
        public async Task GetUserAsyncNegativeTest()
        {
            // Arrange
            var user = new User()
            {
                Id = "test-id",
                Email = "test@emial.com",
                FirstName = "John",
                LastName = "Doe"
            };
            var userDto = new UserDto()
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
            
            // Act
            var result = await _userService.GetUserAsync("test-id");

            // Assert
            Assert.NotNull(result);
            Assert.False(result.Succeeded);
            Assert.NotEqual(result.Error, ResultError.None);
        }

        [Fact]
        public async Task GetUserIdByEmailAsyncPositiveTest()
        {
            // Arrange
            var user = new User()
            {
                Id = "test-id",
                Email = "test@emial.com",
                FirstName = "John",
                LastName = "Doe"
            };
            _userManagerMock.Setup(u => u.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);

            // Act
            var result = await _userService.GetUserIdByEmailAsync("test@emial.com");

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Succeeded);
            Assert.Equal(result.Error, ResultError.None);
            Assert.Equal(result.Value, user.Id);
        }

        [Fact]
        public async Task GetUserIdByEmailAsyncNegativeTest()
        {
            // Arrange
            var user = new User()
            {
                Id = "test-id",
                Email = "test@emial.com",
                FirstName = "John",
                LastName = "Doe"
            };
            _userManagerMock.Setup(u => u.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(user);

            // Act
            var result = await _userService.GetUserIdByEmailAsync("test@emial.com");

            // Assert
            Assert.NotNull(result);
            Assert.False(result.Succeeded);
            Assert.NotEqual(result.Error, ResultError.None);
        }

        [Fact]
        public async Task ListUsersStocksAsyncTest()
        {
            // Arrange 
            var stockList = new List<Stock>()
            {
                new Stock() { Id = 1, Name = "TestStock1"},
                new Stock() { Id = 2, Name = "TestStock2" }
            };
            _mapperMock.Setup(m => m.Map<ViewStockDtoForList>(It.IsAny<Stock>())).Returns<Stock>(s => new ViewStockDtoForList() { Name = s.Name });
            _userRepositoryMock.Setup(u => u.ListUsersStocksAsync(It.IsAny<string>())).ReturnsAsync(stockList);

            // Act
            var result = await _userService.ListUsersStocksAsync("user-id");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result[0].Name, stockList[0].Name);
            Assert.Equal(result[1].Name, stockList[1].Name);
        }
    }
}
