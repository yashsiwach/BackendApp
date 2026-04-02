using System.Security.Cryptography;
using AuthService.Application.Interfaces;
using AuthService.Application.Interfaces.Repositories;
using AuthService.Domain.Entities;
using MassTransit;
using SharedContracts.Events;

namespace AuthService.Application.Services;

public class OTPServiceImpl : IOTPService
{
    private readonly IOTPRepository _otps;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<OTPServiceImpl> _logger;

    /// <summary>Initializes OTP service dependencies for user lookup, OTP storage, and logging.</summary>
    public OTPServiceImpl(IOTPRepository otps, IPublishEndpoint publishEndpoint, ILogger<OTPServiceImpl> logger)
    {
        _otps = otps;
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    /// <summary>Generates a time-limited OTP for the user email and persists it for later verification.</summary>
    public async Task SendOTPAsync(string email)
    {
        var code = RandomNumberGenerator.GetInt32(100000, 1000000).ToString();

        await _otps.AddAsync(new OTPLog
        {
            Email = email,
            Code = code,
            ExpiresAt = DateTime.UtcNow.AddMinutes(5)
        });
        await _otps.SaveAsync();

        await _publishEndpoint.Publish(new OtpGenerated
        {
            Email = email,
            Code = code,
            ExpiresAt = DateTime.UtcNow.AddMinutes(5),
            OccurredAt = DateTime.UtcNow
        });

        _logger.LogInformation("OTP sent to {Email}", email);
    }

    /// <summary>Validates the provided OTP code for the user email, marks it used, and returns verification result.</summary>
    public async Task<bool> VerifyOTPAsync(string email, string code)
    {
        var otp = await _otps.FindValidAsync(email, code);
        if (otp == null) return false;

        otp.UsedAt = DateTime.UtcNow;
        await _otps.SaveAsync();

        _logger.LogInformation("OTP verified for {Email}", email);
        return true;
    }
}
