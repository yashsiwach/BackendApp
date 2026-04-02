using AuthService.Application.DTOs;
using AuthService.Application.Interfaces;
using AuthService.Application.Interfaces.Repositories;
using AuthService.Application.Mappers;
using AuthService.Domain.Entities;
using MassTransit;
using SharedContracts.Events;

namespace AuthService.Application.Services;

public class AuthServiceImpl : IAuthService
{
    private readonly IUserRepository _users;
    private readonly IRefreshTokenRepository _refreshTokens;
    private readonly ITokenService _tokenService;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<AuthServiceImpl> _logger;
    private readonly IOTPService _otpService;

    /// <summary>Initializes authentication dependencies for user, token, and event publishing operations.</summary>
    public AuthServiceImpl(IUserRepository users,IRefreshTokenRepository refreshTokens,ITokenService tokenService,IPublishEndpoint publishEndpoint,ILogger<AuthServiceImpl> logger, IOTPService otpService)
    {
        _users           = users;
        _refreshTokens   = refreshTokens;
        _tokenService    = tokenService;
        _publishEndpoint = publishEndpoint;
        _logger          = logger;
        _otpService      = otpService;
    }

    /// <summary>Registers a new user, stores credentials, publishes registration event, and returns access and refresh tokens.</summary>
    public async Task<AuthResponse> SignupAsync(SignupRequest request)
    {
        var exists = await _users.ExistsAsync(request.Email, request.Phone);
        if (exists)
            throw new InvalidOperationException("User with this email or phone already exists.");

        if (string.IsNullOrWhiteSpace(request.Code))
            throw new UnauthorizedAccessException("OTP code is required.");

        var verified = await _otpService.VerifyOTPAsync(request.Email, request.Code);
        if (!verified)
            throw new UnauthorizedAccessException("Invalid or expired OTP.");

        var user = new User
        {
            Email        = request.Email,
            Phone        = request.Phone,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Role         = "User"
        };

        await _users.AddAsync(user);
        await _users.SaveAsync();

        _logger.LogInformation("User registered: {UserId} {Email}", user.Id, user.Email);

        await _publishEndpoint.Publish(new UserRegistered
        {
            UserId     = user.Id,
            Email      = user.Email,
            Phone      = user.Phone,
            Role       = user.Role,
            OccurredAt = DateTime.UtcNow
        });

        return await IssueTokenPairAsync(user);
    }

    /// <summary>Validates user credentials and active status, then issues a fresh access and refresh token pair.</summary>
    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var user = await _users.FindByEmailAsync(request.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid email or password.");

        if (!user.IsActive)
            throw new UnauthorizedAccessException("Account is deactivated. Contact support.");

        _logger.LogInformation("User logged in: {UserId}", user.Id);

        return await IssueTokenPairAsync(user);
    }

    /// <summary>Revokes the provided refresh token if valid and returns a newly issued access and refresh token pair.</summary>
    public async Task<AuthResponse> RefreshTokenAsync(string refreshToken)
    {
        var stored = await _refreshTokens.FindActiveByTokenAsync(refreshToken);
        if (stored == null || stored.ExpiresAt < DateTime.UtcNow)
            throw new UnauthorizedAccessException("Invalid or expired refresh token.");

        stored.Revoked = true;
        await _refreshTokens.SaveAsync();

        return await IssueTokenPairAsync(stored.User);
    }

    /// <summary>Revokes the specified active refresh token for the user to complete logout.</summary>
    public async Task LogoutAsync(Guid userId, string refreshToken)
    {
        var token = await _refreshTokens.FindActiveByUserAndTokenAsync(userId, refreshToken);
        if (token != null)
        {
            token.Revoked = true;
            await _refreshTokens.SaveAsync();
        }

        _logger.LogInformation("User logged out: {UserId}", userId);
    }

    // ──────── Private helpers ────────

    /// <summary>Generates access and refresh tokens, persists the refresh token, and maps the auth response payload.</summary>
    private async Task<AuthResponse> IssueTokenPairAsync(User user)
    {
        var (accessToken, expiresAt) = _tokenService.GenerateAccessToken(user);

        var refreshToken = new RefreshToken
        {
            UserId    = user.Id,
            Token     = _tokenService.GenerateRefreshToken(),
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        };

        await _refreshTokens.AddAsync(refreshToken);
        await _refreshTokens.SaveAsync();

        return AuthMapper.ToAuthResponse(accessToken, refreshToken.Token, expiresAt, user);
    }
}
