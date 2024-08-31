using Newtonsoft.Json.Linq;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using CurrencyConverterAPI.Controllers;
using CurrencyConverterAPI.Services;
using CurrencyConverterAPI.Classes;

namespace CurrencyConverterAPI.Tests
{
    public class UserControllerTests
    {
        private readonly Mock<IConfiguration> _mockConfig;
        private readonly Mock<IUserService> _mockUserService;

        public UserControllerTests()
        {
            var mockConfigData = new Dictionary<string, string>
            {
                { "Jwt:Key", "JWTAuthenticationHIGHsecuredPasswordVVVp1OH7Xzyr" },
                { "Jwt:Audience", "https://localhost" },
                { "Jwt:Issuer", "https://localhost" }
            };

            _mockConfig = new Mock<IConfiguration>();

            foreach (var kvp in mockConfigData)
            {
                var section = new Mock<IConfigurationSection>();
                section.Setup(s => s.Value).Returns(kvp.Value);
                _mockConfig.Setup(config => config.GetSection(kvp.Key)).Returns(section.Object);
            }

            _mockUserService = new Mock<IUserService>();
        }

        [Fact]
        public void GenerateToken_ShouldReturnOk_WhenCredentialsAreValid()
        {
            // Arrange
            var validUser = new UserLogin { Username = "admin", Password = "password" };
            var user = new User { Id = 1, Username = "admin", Email = "email@example.com" };

            _mockUserService.Setup(service => service.ValidateUser(validUser)).Returns(user);

            var controller = new UserController(_mockUserService.Object);

            // Act
            var result = controller.GenerateToken(validUser);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);

            // Cast the result to JObject for safe property access
            var jsonObject = okResult.Value ;
            Assert.NotNull(jsonObject);
        }

        [Fact]
        public void GenerateToken_ShouldReturnUnauthorized_WhenCredentialsAreInvalid()
        {
            // Arrange
            var invalidUser = new UserLogin { Username = "admin", Password = "wrongpassword" };

            _mockUserService.Setup(service => service.ValidateUser(invalidUser)).Returns((User)null);

            var controller = new UserController(_mockUserService.Object);

            // Act
            var result = controller.GenerateToken(invalidUser);

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public void GenerateToken_ShouldReturnBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            var controller = new UserController(_mockUserService.Object);
            controller.ModelState.AddModelError("Username", "Username is required");

            var invalidUser = new UserLogin { Username = "", Password = "password" };

            // Act
            var result = controller.GenerateToken(invalidUser);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
