using CurrencyConverterAPI.Classes;
using CurrencyConverterAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyConverterAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize ]// Require authentication for all actions in this controller
    public class CurrencyController : ControllerBase
    {
        private readonly ICurrencyService _frankfurterService;

        public CurrencyController(ICurrencyService currencyService)
        {
            _frankfurterService = currencyService;
        }

        [HttpGet("latest")]
        public async Task<IActionResult> GetLatestRates([FromQuery] string baseCurrency)
        {
            // Validate base currency
            if (!Helper.IsValidCurrency(baseCurrency))
                return BadRequest("Invalid base currency.");

            var result = await _frankfurterService.GetLatestRates(baseCurrency);
            if (result == null) return BadRequest("Unable to retrieve latest rates.");
            return Ok(result);
        }

        [HttpGet("convert")]
        public async Task<IActionResult> ConvertCurrency([FromQuery] string from, [FromQuery] string to, [FromQuery] decimal amount)
        {
            // Validate input currencies and amount
            if (!Helper.IsValidCurrency(from) || !Helper.IsValidCurrency(to))
                return BadRequest("Invalid currency code.");
            if (amount <= 0)
                return BadRequest("Amount must be greater than zero.");

            var result = await _frankfurterService.ConvertCurrency(from, to, amount);
            if (result == null) return BadRequest("Currency conversion failed or unsupported currency.");
            return Ok(result);
        }

        [HttpGet("historical")]
        public async Task<IActionResult> GetHistoricalRates([FromQuery] string baseCurrency, [FromQuery] string startDate, [FromQuery] string endDate)
        {
            // Validate base currency and date range
            if (!Helper.IsValidCurrency(baseCurrency))
                return BadRequest("Invalid base currency.");
            if (!Helper.IsValidDateFormat(startDate) || !Helper.IsValidDateFormat(endDate))
                return BadRequest("Invalid date format. Use YYYY-MM-DD.");
            if (DateTime.Parse(startDate) > DateTime.Parse(endDate))
                return BadRequest("Start date must be before end date.");

            var result = await _frankfurterService.GetHistoricalRates(baseCurrency, startDate, endDate);
            if (result == null) return BadRequest("Unable to retrieve historical rates.");
            return Ok(result);
        }
    }
}
