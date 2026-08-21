using API.Exceptions;
using API.Interfaces.Service;
using API.Services;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Xunit;

namespace API.Tests.Services;

public class CurrentUserServiceTests
{
    [Fact]
    public async Task ClaimsAndRoles_ShouldBeReadFromHttpContext()
    {
        using var host = new API.Tests.Integration.AuthTestHost();
        var contextAccessor = new HttpContextAccessor
        {
            HttpContext = CreateContext(host.User.Id, host.User.EmployeeId, "Admin")
        };
        var service = new CurrentUserService(
            contextAccessor,
            host.UserManager,
            new Moq.Mock<IPermissionService>().Object);

        Assert.Equal(host.User.Id, service.UserId);
        Assert.Equal(host.User.EmployeeId, service.EmployeeId);
        Assert.True(service.IsAuthenticated);
        Assert.True(service.IsInRole("Admin"));
        Assert.False(service.IsInRole("Employee"));
    }

    [Fact]
    public async Task GetCurrentUserAsync_ShouldReturnUserFromClaims()
    {
        using var host = new API.Tests.Integration.AuthTestHost();
        var contextAccessor = new HttpContextAccessor
        {
            HttpContext = CreateContext(host.User.Id, host.User.EmployeeId)
        };
        var service = new CurrentUserService(
            contextAccessor,
            host.UserManager,
            new Moq.Mock<IPermissionService>().Object);

        var result = await service.GetCurrentUserAsync();

        Assert.Equal(host.User.Id, result.Id);
        Assert.Equal(host.User.Email, result.Email);
    }

    [Fact]
    public async Task GetCurrentUserAsync_ShouldThrowUnauthorizedWithoutUserId()
    {
        using var host = new API.Tests.Integration.AuthTestHost();
        var service = new CurrentUserService(
            new HttpContextAccessor { HttpContext = new DefaultHttpContext() },
            host.UserManager,
            new Moq.Mock<IPermissionService>().Object);

        await Assert.ThrowsAsync<UnauthorizedException>(() => service.GetCurrentUserAsync());
    }

    private static DefaultHttpContext CreateContext(int employeeId, int currentEmployeeId, string? role = null)
    {
        return CreateContext(employeeId.ToString(), currentEmployeeId, role);
    }

    private static DefaultHttpContext CreateContext(string userId, int employeeId, string? role = null)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId),
            new("EmployeeId", employeeId.ToString())
        };
        if (role != null)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        return new DefaultHttpContext
        {
            User = new ClaimsPrincipal(new ClaimsIdentity(claims, "TestAuth"))
        };
    }
}