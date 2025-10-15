using System.Data;
using Tech.Application.Auth.DTOs;
using Tech.Application.Auth.Interfaces;
using Tech.Core.Auth.Common;
using Tech.Core.Auth.Entities;
using Tech.Core.Transactions;
using Tech.Core.Auth.Common.Result;
using Tech.Core.Auth.Enums;
using Tech.Core.Auth.Common.Exceptions;
using System.Reflection.Metadata.Ecma335;

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
                var messageResult = await _registerCompanyRepository.ExistCompanyAsync(request.CompanyName, request.Email);
                
                if (messageResult is not null) return Result<AuthResponse>.Fail(messageResult, ErrorType.AlreadyExists);

                try
                {
                    Result<Company> companyResult = Company.Create(request.CompanyName, request.Email, request.SubscriptionId, 0);
                    if (!companyResult.IsSuccess) return Result<AuthResponse>.Fail(companyResult.Errors.First().Message, ErrorType.Validation);

                    var company = companyResult.Value!;
                    await _companyRepository.AddAsync(company);
                    await _companyRepository.SaveChanges();

                    Result<User> userResult = User.Create(company.Id, request.Email, request.Password, request.FirstName, request.LastName, true);
                    if (!userResult.IsSuccess) return Result<AuthResponse>.Fail(userResult.Errors.First().Message, ErrorType.Validation);

                    var admin = userResult.Value!;
                    await _userRepository.AddAsync(admin);
                    await _userRepository.SaveChanges();

                    company.CompanyAdminId = admin.Id;

                    var initializedRelationsTask = _registerCompanyRepository.InitializeCompanyAndUserRelationsAsync(company, admin, request.SubscriptionId);

                    (Result<RefreshToken> refreshTokenResult, string accessToken, string refreshToken) = _jwtService.GenerateToken(admin);
                    if (!refreshTokenResult.IsSuccess) return Result<AuthResponse>.Fail(refreshTokenResult.Errors.First().Message, ErrorType.Validation);
                    var refreshTokenEntity = refreshTokenResult.Value!;

                    await initializedRelationsTask;

                    await _refreshTokenRepository.AddAsync(refreshTokenEntity, admin.Id);
                    await _refreshTokenRepository.SaveChanges();

                    return Result<AuthResponse>.Ok(new AuthResponse
                    {
                        AccessToken = accessToken,
                        RefreshToken = refreshToken
                    });

                }
                catch (Exception)
                {
                    throw;
                }   
            }, IsolationLevel.ReadCommitted, cancellationToken);
        }

        public async Task<Result<AuthResponse>> LoginAsync(LoginRequest request)
        {
            try
            {
                var user = await _userRepository.GetByEmailAsync(request.Email);
                if (user == null || !user.VerifyPassword(request.Password))
                    return Result<AuthResponse>.Fail("Invalid email or password.", ErrorType.Unauthorized);

                (Result<RefreshToken> refreshTokenResult, string accessToken, string refreshToken) = _jwtService.GenerateToken(user);
                if (!refreshTokenResult.IsSuccess) return Result<AuthResponse>.Fail(refreshTokenResult.Errors.First().Message, ErrorType.Validation);
                var refreshTokenEntity = refreshTokenResult.Value!;

                await _refreshTokenRepository.AddAsync(refreshTokenEntity, user.Id);

                return Result<AuthResponse>.Ok(new AuthResponse { AccessToken = accessToken, RefreshToken = refreshToken });
            }
            catch (Exception ex)
            {
                return Result<AuthResponse>.Fail($"Failed to login: {ex.Message}", ErrorType.Internal);
            }
        }

        public async Task<Result<AuthResponse>> RefreshTokenAsync(string refreshToken)
        {
            try
            {
                var userId = await _jwtService.ValidateRefreshTokenAsync(refreshToken);
                if (userId == null)
                    return Result<AuthResponse>.Fail("Invalid or expired refresh token.", ErrorType.Unauthorized);

                var user = await _userRepository.GetByIdAsync(userId.Value);
                if (user == null)
                    return Result<AuthResponse>.Fail("User not found.", ErrorType.NotFound);

                (Result<RefreshToken> refreshTokenResult, string accessToken, string newRefreshToken) = _jwtService.GenerateToken(user);
                if (!refreshTokenResult.IsSuccess) return Result<AuthResponse>.Fail(refreshTokenResult.Errors.First().Message, ErrorType.Validation);
                var refreshTokenEntity = refreshTokenResult.Value!;

                await _refreshTokenRepository.AddAsync(refreshTokenEntity, userId.Value);

                return Result<AuthResponse>.Ok(new AuthResponse { AccessToken = accessToken, RefreshToken = newRefreshToken });
            }
            catch (Exception ex)
            {
                return Result<AuthResponse>.Fail($"Failed to refresh token: {ex.Message}", ErrorType.Internal);
            }
        }
    }
}
