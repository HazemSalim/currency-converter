using CurrencyConverterAPI.Classes;

namespace CurrencyConverterAPI.Services
{
    public interface IUserService
    {
        User ValidateUser(UserLogin request);

        string CreateToken(UserLogin request);

        
    }
}
