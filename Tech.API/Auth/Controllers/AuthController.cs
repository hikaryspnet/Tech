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
        public async Task<IActionResult> Register([FromBody] RegisterCompanyRequest request, CancellationToken cancellationToken)
        {
            var result = await _authService.RegisterCompanyAsync(request, cancellationToken);
            if (!result.IsSuccess)
            {
                var error = result.Errors.First();

                return error.Type switch
                {
                    Core.Auth.Enums.ErrorType.AlreadyExists => Conflict(error.Message),
                    Core.Auth.Enums.ErrorType.NotFound => NotFound(error.Message),
                    Core.Auth.Enums.ErrorType.Validation => BadRequest(error.Message),
                    Core.Auth.Enums.ErrorType.Unauthorized => Unauthorized(error.Message),
                    _ => StatusCode(500, error.Message)
                };
            }

            return Ok(result.Value);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _authService.LoginAsync(request);

            if (!result.IsSuccess)
            {
                var error = result.Errors.First();

                return error.Type switch
                {
                    Core.Auth.Enums.ErrorType.AlreadyExists => Conflict(error.Message),
                    Core.Auth.Enums.ErrorType.NotFound => NotFound(error.Message),
                    Core.Auth.Enums.ErrorType.Validation => BadRequest(error.Message),
                    Core.Auth.Enums.ErrorType.Unauthorized => Unauthorized(error.Message),
                    _ => StatusCode(500, error.Message)
                };
            }

            return Ok(result.Value);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest refreshTokenRequest)
        {
            var result = await _authService.RefreshTokenAsync(refreshTokenRequest.RefreshToken);

            if (!result.IsSuccess)
            {
                var error = result.Errors.First();

                return error.Type switch
                {
                    Core.Auth.Enums.ErrorType.AlreadyExists => Conflict(error.Message),
                    Core.Auth.Enums.ErrorType.NotFound => NotFound(error.Message),
                    Core.Auth.Enums.ErrorType.Validation => BadRequest(error.Message),
                    Core.Auth.Enums.ErrorType.Unauthorized => Unauthorized(error.Message),
                    _ => StatusCode(500, error.Message)
                };
            }

            return Ok(result.Value);
        }
    }
}
