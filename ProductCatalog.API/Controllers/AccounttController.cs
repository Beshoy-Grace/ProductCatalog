using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Application.IServices;
using ProductCatalog.Common.User.Request;

namespace ProductCatalog.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
      
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CreateUserDTO registerReqDTO)
        {
            var result = await _userService.CreateUserAsync(registerReqDTO);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginReqDTO loginReqDTO)
        {
            var result = await _userService.LoginAsync(loginReqDTO);
            return Ok(result);
        }


    }
}
