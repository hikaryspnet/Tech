using Microsoft.AspNetCore.Mvc;
using Tech.API.Common;
using Tech.Application.Auth.DTOs;
using Tech.Application.Auth.Interfaces;

namespace Tech.API.Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterCompanyRequest request)
        {
            var result = await _authService.RegisterCompanyAsync(request, CancellationToken.None);
            if (result.IsFailed)
                return BadRequest(result.Errors.First().Message);

            return Ok(result.Value);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _authService.LoginAsync(request);
            if (result.IsFailed)
                return BadRequest(result.Errors.First().Message);

            return Ok(result.Value);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest refreshTokenRequest)
        {
            var result = await _authService.RefreshTokenAsync(refreshTokenRequest.RefreshToken);
            if (result.IsFailed)
                return BadRequest(result.Errors.First().Message);

            return Ok(result.Value);
        }
    }
}
