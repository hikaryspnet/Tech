using FluentResults;
using Tech.Application.Auth.DTOs;

namespace Tech.Application.Auth.Interfaces
{
    public interface IAuthService
    {
        Task<Result<AuthResponse>> RegisterCompanyAsync(RegisterCompanyRequest request, CancellationToken cancellationToken);
        Task<Result<AuthResponse>> LoginAsync(LoginRequest request);
        Task<Result<AuthResponse>> RefreshTokenAsync(string refreshToken);
    }
}
