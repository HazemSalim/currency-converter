using Microsoft.Extensions.Caching.Memory; // For caching

namespace CurrencyConverterAPI.Services
{
    public class FrankfurterService : ICurrencyService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<FrankfurterService> _logger;
        private readonly IMemoryCache _cache; // Add memory cache for caching responses
        private const string BaseUrl = "https://api.frankfurter.app/";

        public FrankfurterService(HttpClient httpClient, ILogger<FrankfurterService> logger, IMemoryCache cache)
        {
            _httpClient = httpClient;
            _logger = logger;
            _cache = cache;
        }

        public async Task<string> GetLatestRates(string baseCurrency)
        {
            var cacheKey = $"LatestRates_{baseCurrency}";

            // Check if the data is already cached
            if (_cache.TryGetValue(cacheKey, out string cachedRates))
            {
                _logger.LogInformation($"Cache hit for {cacheKey}");
                return cachedRates;
            }

            var url = $"{BaseUrl}latest?base={baseCurrency}";
            int maxRetries = 3;
            int retries = 0;

            while (retries < maxRetries)
            {
                try
                {
                    var response = await _httpClient.GetAsync(url);
                    response.EnsureSuccessStatusCode();

                    var content = await response.Content.ReadAsStringAsync();

                    // Cache the result for 5 minutes
                    _cache.Set(cacheKey, content, TimeSpan.FromMinutes(5));

                    return content;
                }
                catch (HttpRequestException ex)
                {
                    retries++;
                    _logger.LogError($"Attempt {retries} to request {url} failed: {ex.Message}");
                    await Task.Delay(200); // Wait for 200ms before retrying
                }
            }

            return null; // Return null if all retries fail
        }

        public async Task<string> GetHistoricalRates(string baseCurrency, string startDate, string endDate)
        {
            var cacheKey = $"HistoricalRates_{baseCurrency}_{startDate}_{endDate}";

            // Check if the data is already cached
            if (_cache.TryGetValue(cacheKey, out string cachedRates))
            {
                _logger.LogInformation($"Cache hit for {cacheKey}");
                return cachedRates;
            }

            var url = $"{BaseUrl}{startDate}..{endDate}?base={baseCurrency}";
            int maxRetries = 3;
            int retries = 0;

            while (retries < maxRetries)
            {
                try
                {
                    var response = await _httpClient.GetAsync(url);
                    response.EnsureSuccessStatusCode();

                    var content = await response.Content.ReadAsStringAsync();

                    // Cache the result for 10 minutes
                    _cache.Set(cacheKey, content, TimeSpan.FromMinutes(10));

                    return content;
                }
                catch (HttpRequestException ex)
                {
                    retries++;
                    _logger.LogError($"Attempt {retries} to request {url} failed: {ex.Message}");
                    await Task.Delay(200); // Wait for 200ms before retrying
                }
            }

            return null; // Return null if all retries fail
        }

        public async Task<string> ConvertCurrency(string from, string to, decimal amount)
        {
            if (new[] { "TRY", "PLN", "THB", "MXN" }.Contains(to))
                return null; // Handle excluded currencies

            var cacheKey = $"ConvertCurrency_{from}_{to}_{amount}";

            // Check if the data is already cached
            if (_cache.TryGetValue(cacheKey, out string cachedConversion))
            {
                _logger.LogInformation($"Cache hit for {cacheKey}");
                return cachedConversion;
            }

            var url = $"{BaseUrl}latest?amount={amount}&from={from}&to={to}";
            int maxRetries = 3;
            int retries = 0;

            while (retries < maxRetries)
            {
                try
                {
                    var response = await _httpClient.GetAsync(url);
                    response.EnsureSuccessStatusCode();

                    var content = await response.Content.ReadAsStringAsync();

                    // Cache the result for 5 minutes
                    _cache.Set(cacheKey, content, TimeSpan.FromMinutes(5));

                    return content;
                }
                catch (HttpRequestException ex)
                {
                    retries++;
                    _logger.LogError($"Attempt {retries} to request {url} failed: {ex.Message}");
                    await Task.Delay(200); // Wait for 200ms before retrying
                }
            }

            return null; // Return null if all retries fail
        }
    }
}
