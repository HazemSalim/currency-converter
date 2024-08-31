Currency Converter API

Description:

This API provides functionality for currency conversion using the Frankfurter API. It allows users to:

Retrieve the latest exchange rates for a specific base currency.
Convert amounts between different currencies, with certain exclusions.
Get historical exchange rates for a specified period.

----------------------------------------------------------------------------
 
Setup:

Clone the Repository:
Clone the repository to your local machine.

Install Dependencies:
Navigate to the project directory and restore the dependencies using dotnet restore.

Start the Application:
Run the application

Endpoints:


POST /api/user/generate-token
Generates a JWT token for authentication. Requires a username and password in the request body.
username=admin and password=password for simplicity

-------------

GET /api/currency/latest?base=EUR
Retrieves the latest exchange rates for the specified base currency.

GET /api/currency/convert?from=USD&to=EUR&amount=100
Converts the specified amount from one currency to another. Requests involving excluded currencies (TRY, PLN, THB, MXN) will return a BadRequest.

GET /api/currency/historical?base=EUR&startDate=2020-01-01&endDate=2020-01-31
Retrieves historical exchange rates for the specified base currency and date range.

----------------------------------------------------------------

Assumptions:

The Frankfurter API may not respond on the first request; retry logic is implemented.
Currencies TRY, PLN, THB, and MXN are excluded from conversion. Requests involving these currencies will return a BadRequest.
JWT tokens are used to secure API endpoints.
Improvements:

Caching: Implemented to reduce the number of API calls.
Error Handling and Logging: Enhanced for better diagnostics.
Rate Limiting: Implemented to manage traffic and avoid overwhelming the API.
Security: JWT tokens are used for securing endpoints.


Security Token Steps:

Generate Token:
Send a POST request to /api/user/generate-token with a body containing username and password. If the credentials are valid, a JWT token will be returned.

Use Token:
Include the JWT token in the Authorization header of requests to protected endpoints. Example header:

Authorization: Bearer <your-jwt-token>

--------------------------------------------------------------
 
Running Tests:

To ensure the functionality and security of the API, run the unit tests using dotnet test 
for the other project (CurrencyConverterAPI.Tests).

Test Cases:

GenerateToken_ShouldReturnOk_WhenCredentialsAreValid
Validates that a JWT token is successfully generated when valid credentials are provided.

GenerateToken_ShouldReturnUnauthorized_WhenCredentialsAreInvalid
Checks response when invalid credentials are provided.

GetLatestRates_ShouldReturnOk
Validates successful retrieval of the latest exchange rates.

GetLatestRates_ShouldReturnBadRequest_WhenServiceFails
Checks response when the service fails to provide rates.

ConvertCurrency_ShouldReturnBadRequest_WhenCurrencyExcluded
Ensures excluded currencies result in a BadRequest.

ConvertCurrency_ShouldReturnOk_WhenValidCurrencyConversion
Validates successful currency conversion for allowed currencies.

GetHistoricalRates_ShouldReturnOk
Confirms successful retrieval of historical rates for a given date range.
