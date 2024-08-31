using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using CurrencyConverterAPI.Controllers;
using CurrencyConverterAPI.Services;

namespace CurrencyConverterAPI.Tests
{
    public class CurrencyConverterControllerTests
    {
        private readonly Mock<IConfiguration> _mockConfig;

        public CurrencyConverterControllerTests()
        {
            // Set up a mock configuration with a 32-byte key (256 bits)
            _mockConfig = new Mock<IConfiguration>();
            _mockConfig.Setup(config => config["Jwt:Key"]).Returns("JWTAuthenticationHIGHsecuredPasswordVVVp1OH7Xzyr!");
        }

        [Fact]
        public async Task GetLatestRates_ShouldReturnOk()
        {
            // Arrange
            var mockCurrencyService = new Mock<ICurrencyService>();
            mockCurrencyService.Setup(service => service.GetLatestRates(It.IsAny<string>()))
                .ReturnsAsync("{\"base\":\"EUR\",\"rates\":{\"USD\":1.1}}");

            var controller = new CurrencyController(mockCurrencyService.Object);

            // Act
            var result = await controller.GetLatestRates("EUR");

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetLatestRates_ShouldReturnBadRequest_WhenServiceFails()
        {
            // Arrange
            var mockCurrencyService = new Mock<ICurrencyService>();
            mockCurrencyService.Setup(service => service.GetLatestRates(It.IsAny<string>()))
                .ReturnsAsync((string)null);

            var controller = new CurrencyController(mockCurrencyService.Object);

            // Act
            var result = await controller.GetLatestRates("INVALID_CURRENCY");

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task ConvertCurrency_ShouldReturnBadRequest_WhenCurrencyExcluded()
        {
            // Arrange
            var mockCurrencyService = new Mock<ICurrencyService>();
            mockCurrencyService.Setup(service => service.ConvertCurrency("EUR", "TRY", 100))
                .ReturnsAsync((string)null); // Excluded currencies return null

            var controller = new CurrencyController(mockCurrencyService.Object);

            // Act
            var result = await controller.ConvertCurrency("EUR", "TRY", 100);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task ConvertCurrency_ShouldReturnOk_WhenValidCurrencyConversion()
        {
            // Arrange
            var mockCurrencyService = new Mock<ICurrencyService>();
            mockCurrencyService.Setup(service => service.ConvertCurrency("EUR", "USD", 100))
                .ReturnsAsync("{\"amount\":100,\"from\":\"EUR\",\"to\":\"USD\",\"rate\":1.1}");

            var controller = new CurrencyController(mockCurrencyService.Object);

            // Act
            var result = await controller.ConvertCurrency("EUR", "USD", 100);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetHistoricalRates_ShouldReturnOk()
        {
            // Arrange
            var mockCurrencyService = new Mock<ICurrencyService>();
            mockCurrencyService.Setup(service => service.GetHistoricalRates("EUR", "2023-01-01", "2023-01-31"))
                .ReturnsAsync("{\"base\":\"EUR\",\"rates\":{\"2023-01-01\":{\"USD\":1.1},\"2023-01-31\":{\"USD\":1.2}}}");

            var controller = new CurrencyController(mockCurrencyService.Object);

            // Act
            var result = await controller.GetHistoricalRates("EUR", "2023-01-01", "2023-01-31");

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }
    }
}
