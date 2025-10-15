using Tech.Application.Auth.DTOs;
using Tech.Core.Auth.Common.Result;

namespace Tech.Application.Auth.Interfaces
{
    public interface IAuthService
    {
        Task<Result<AuthResponse>> RegisterCompanyAsync(RegisterCompanyRequest request, CancellationToken cancellationToken);
        Task<Result<AuthResponse>> LoginAsync(LoginRequest request);
        Task<Result<AuthResponse>> RefreshTokenAsync(string refreshToken);
    }
}
