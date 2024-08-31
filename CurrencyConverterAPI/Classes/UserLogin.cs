using System.ComponentModel.DataAnnotations;

namespace CurrencyConverterAPI.Classes
{
    public class UserLogin
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
