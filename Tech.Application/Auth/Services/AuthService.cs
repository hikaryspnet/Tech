using FluentResults;
using System.Data;
using Tech.Application.Auth.DTOs;
using Tech.Application.Auth.Interfaces;
using Tech.Core.Auth.Common;
using Tech.Core.Auth.Entities;
using Tech.Core.Transactions;

namespace Tech.Application.Auth.Services
{
    public class AuthService : IAuthService
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly ITransactionalExecutor _transactionalExecutor;
        private readonly IRegisterCompanyRepository _registerCompanyRepository;

        public AuthService(
            ICompanyRepository companyRepository,
            IUserRepository userRepository,
            IJwtService jwtService,
            IRefreshTokenRepository refreshTokenRepository,
            ITransactionalExecutor transactionalExecutor,
            IRegisterCompanyRepository registerCompanyRepository)
        {
            _companyRepository = companyRepository;
            _userRepository = userRepository;
            _jwtService = jwtService;
            _refreshTokenRepository = refreshTokenRepository;
            _transactionalExecutor = transactionalExecutor;
            _registerCompanyRepository = registerCompanyRepository;
        }

        public async Task<Result<AuthResponse>> RegisterCompanyAsync(RegisterCompanyRequest request, CancellationToken cancellationToken = default)
        {
            return await _transactionalExecutor.StartEffect(async ct =>
            {
                var result = await _registerCompanyRepository.ExistCompanyAsync(request.CompanyName, request.Email);
                
                if (result is not null) return Result.Fail(result);

                try
                {
                    var company = Company.Create(request.CompanyName, request.Email, request.SubscriptionId, 0);
                    await _companyRepository.AddAsync(company);
                    await _companyRepository.SaveChanges();

                    var admin = User.Create(company.Id, request.Email, request.Password, request.FirstName, request.LastName, true);
                    await _userRepository.AddAsync(admin);
                    await _userRepository.SaveChanges();

                    company.CompanyAdminId = admin.Id;

                    await _registerCompanyRepository.InitializeCompanyAndUserRelationsAsync(company, admin, request.SubscriptionId);

                    var (accessToken, refreshToken, refreshTokenEntity) = await _jwtService.GenerateTokensAsync(admin);
                    await _refreshTokenRepository.AddAsync(refreshTokenEntity);
                    await _refreshTokenRepository.SaveChanges();

                    return Result.Ok(new AuthResponse
                    {
                        AccessToken = accessToken,
                        RefreshToken = refreshToken
                    });

                }
                catch (Exception ex)
                {
                    throw ex;
                }   
            }, IsolationLevel.ReadCommitted, cancellationToken);
        }

        public async Task<Result<AuthResponse>> LoginAsync(LoginRequest request)
        {
            try
            {
                var user = await _userRepository.GetByEmailAsync(request.Email);
                if (user == null || !user.VerifyPassword(request.Password))
                    return Result.Fail("Invalid email or password.");

                var (accessToken, refreshToken, refreshTokenEntity) = await _jwtService.GenerateTokensAsync(user);
                await _refreshTokenRepository.AddAsync(refreshTokenEntity);

                return Result.Ok(new AuthResponse { AccessToken = accessToken, RefreshToken = refreshToken });
            }
            catch (Exception ex)
            {
                return Result.Fail($"Failed to login: {ex.Message}");
            }
        }

        public async Task<Result<AuthResponse>> RefreshTokenAsync(string refreshToken)
        {
            try
            {
                var userId = await _jwtService.ValidateRefreshTokenAsync(refreshToken);
                if (userId == null)
                    return Result.Fail("Invalid or expired refresh token.");

                var user = await _userRepository.GetByIdAsync(userId.Value);
                if (user == null)
                    return Result.Fail("User not found.");

                var (accessToken, newRefreshToken, refreshTokenEntity) = await _jwtService.GenerateTokensAsync(user);
                await _refreshTokenRepository.AddAsync(refreshTokenEntity);

                return Result.Ok(new AuthResponse { AccessToken = accessToken, RefreshToken = newRefreshToken });
            }
            catch (Exception ex)
            {
                return Result.Fail($"Failed to refresh token: {ex.Message}");
            }
        }
    }
}
