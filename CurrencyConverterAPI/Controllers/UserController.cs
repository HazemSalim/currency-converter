using CurrencyConverterAPI.Classes;
using CurrencyConverterAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyConverterAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("generate-token")]
        public IActionResult GenerateToken([FromBody] UserLogin request)
        {
            if (request == null || !ModelState.IsValid)
                return BadRequest("Invalid client request");

            // Validate the user credentials (you need to implement this)
            var user = _userService.ValidateUser(request);
            if (user == null)
                return Unauthorized();

            string token =  _userService.CreateToken(request);

            return Ok(new { Token = token });
        }

    }
}