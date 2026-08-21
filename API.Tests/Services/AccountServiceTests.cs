using API.DTOs.Auth;
using API.Exceptions;
using API.Interfaces.Repository;
using API.Interfaces.Service;
using API.Models.Identity;
using API.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace API.Tests.Services;

public class AccountServiceTests
{
    private const string Password = "Password@123";

    [Fact]
    public async Task LoginAsync_ShouldReturnTokens_WhenCredentialsAreValid()
    {
        using var host = new API.Tests.Integration.AuthTestHost();
        var refreshRepository = new Mock<IRefreshTokenRepository>();
        refreshRepository.Setup(repository => repository.GetByUserIdAsync(host.User.Id))
            .ReturnsAsync((RefreshToken?)null);
        var tokenService = CreateTokenService("access-token", "refresh-token");
        var service = CreateService(host, tokenService, refreshRepository);

        var result = await service.LoginAsync(new LoginDto
        {
            Email = host.User.Email!,
            Password = Password
        });

        Assert.Equal("access-token", result.AccessToken);
        Assert.Equal("refresh-token", result.RefreshToken);
        Assert.Equal(host.User.Email, result.Email);
        refreshRepository.Verify(repository => repository.AddAsync(It.Is<RefreshToken>(token =>
            token.UserId == host.User.Id && token.Token == "refresh-token")), Times.Once);
    }

    [Fact]
    public async Task LoginAsync_ShouldThrowUnauthorized_WhenPasswordIsInvalid()
    {
        using var host = new API.Tests.Integration.AuthTestHost();
        var service = CreateService(host);

        await Assert.ThrowsAsync<UnauthorizedException>(() => service.LoginAsync(new LoginDto
        {
            Email = host.User.Email!,
            Password = "WrongPassword@123"
        }));
    }

    [Fact]
    public async Task LoginAsync_ShouldThrowUnauthorized_WhenUserIsInactive()
    {
        using var host = new API.Tests.Integration.AuthTestHost(isActive: false);
        var service = CreateService(host);

        await Assert.ThrowsAsync<UnauthorizedException>(() => service.LoginAsync(new LoginDto
        {
            Email = host.User.Email!,
            Password = Password
        }));
    }

    [Fact]
    public async Task RefreshAsync_ShouldReturnNewTokensAndRotateStoredToken()
    {
        using var host = new API.Tests.Integration.AuthTestHost();
        var storedToken = new RefreshToken
        {
            UserId = host.User.Id,
            Token = "old-refresh-token",
            CreatedAt = DateTime.UtcNow.AddHours(-1),
            ExpiresAt = DateTime.UtcNow.AddHours(1),
            User = host.User
        };
        var refreshRepository = new Mock<IRefreshTokenRepository>();
        refreshRepository.Setup(repository => repository.GetByTokenAsync(storedToken.Token))
            .ReturnsAsync(storedToken);
        var tokenService = CreateTokenService("new-access-token", "new-refresh-token");
        var service = CreateService(host, tokenService, refreshRepository);

        var result = await service.RefreshAsync(new RefreshRequestDto
        {
            RefreshToken = storedToken.Token
        });

        Assert.Equal("new-access-token", result.AccessToken);
        Assert.Equal("new-refresh-token", result.RefreshToken);
        Assert.Equal("new-refresh-token", storedToken.Token);
        refreshRepository.Verify(repository => repository.UpdateAsync(storedToken), Times.Once);
    }

    [Fact]
    public async Task RefreshAsync_ShouldThrowUnauthorized_WhenTokenDoesNotExist()
    {
        using var host = new API.Tests.Integration.AuthTestHost();
        var refreshRepository = new Mock<IRefreshTokenRepository>();
        refreshRepository.Setup(repository => repository.GetByTokenAsync("invalid-token"))
            .ReturnsAsync((RefreshToken?)null);
        var service = CreateService(host, refreshRepository: refreshRepository);

        await Assert.ThrowsAsync<UnauthorizedException>(() => service.RefreshAsync(new RefreshRequestDto
        {
            RefreshToken = "invalid-token"
        }));
    }

    [Fact]
    public async Task RefreshAsync_ShouldThrowUnauthorized_WhenTokenIsExpired()
    {
        using var host = new API.Tests.Integration.AuthTestHost();
        var storedToken = new RefreshToken
        {
            UserId = host.User.Id,
            Token = "expired-token",
            CreatedAt = DateTime.UtcNow.AddDays(-2),
            ExpiresAt = DateTime.UtcNow.AddMinutes(-1),
            User = host.User
        };
        var refreshRepository = new Mock<IRefreshTokenRepository>();
        refreshRepository.Setup(repository => repository.GetByTokenAsync(storedToken.Token))
            .ReturnsAsync(storedToken);
        var service = CreateService(host, refreshRepository: refreshRepository);

        await Assert.ThrowsAsync<UnauthorizedException>(() => service.RefreshAsync(new RefreshRequestDto
        {
            RefreshToken = storedToken.Token
        }));
    }

    private static Mock<ITokenService> CreateTokenService(string accessToken, string refreshToken)
    {
        var tokenService = new Mock<ITokenService>();
        tokenService.Setup(service => service.CreateTokenAsync(
                It.IsAny<ApplicationUser>(), It.IsAny<IList<string>>()))
            .ReturnsAsync(accessToken);
        tokenService.Setup(service => service.GenerateRefreshToken()).Returns(refreshToken);
        return tokenService;
    }

    private static AccountService CreateService(
        API.Tests.Integration.AuthTestHost host,
        Mock<ITokenService>? tokenService = null,
        Mock<IRefreshTokenRepository>? refreshRepository = null)
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:RefreshTokenExpiryInDays"] = "7"
            })
            .Build();

        return new AccountService(
            host.UserManager,
            host.SignInManager,
            tokenService?.Object ?? CreateTokenService("access-token", "refresh-token").Object,
            new Mock<IPermissionService>().Object,
            refreshRepository?.Object ?? new Mock<IRefreshTokenRepository>().Object,
            new Mock<Microsoft.AspNetCore.Http.IHttpContextAccessor>().Object,
            configuration);
    }
}