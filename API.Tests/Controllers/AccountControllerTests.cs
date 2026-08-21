using API.Controllers;
using API.DTOs.Auth;
using API.Interfaces.Service;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace API.Tests.Controllers;

public class AccountControllerTests
{
    [Fact]
    public async Task Login_ShouldReturn200WithResponse()
    {
        var service = new Mock<IAccountService>();
        var expected = new LoginResponseDto { AccessToken = "access", RefreshToken = "refresh" };
        service.Setup(x => x.LoginAsync(It.IsAny<LoginDto>())).ReturnsAsync(expected);

        var result = await new AccountController(service.Object).Login(new LoginDto());

        var response = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Same(expected, response.Value);
    }

    [Fact]
    public async Task Refresh_ShouldReturn200WithResponse()
    {
        var service = new Mock<IAccountService>();
        var expected = new RefreshResponseDto { AccessToken = "new-access", RefreshToken = "new-refresh" };
        service.Setup(x => x.RefreshAsync(It.IsAny<RefreshRequestDto>())).ReturnsAsync(expected);

        var result = await new AccountController(service.Object).Refresh(new RefreshRequestDto());

        Assert.Same(expected, Assert.IsType<OkObjectResult>(result.Result).Value);
    }
}