using API.Controllers;
using API.DTOs.User;
using API.Interfaces.Service;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace API.Tests.Controllers;

public class UsersControllerTests
{
    [Fact]
    public async Task GetUser_ShouldReturn404_WhenUserDoesNotExist()
    {
        var service = new Mock<IUserService>();
        service.Setup(x => x.GetByIdAsync("missing")).ReturnsAsync((UserDto?)null);

        var result = await new UsersController(service.Object).GetUser("missing");

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task DeactivateUser_ShouldReturn204_WhenUpdated()
    {
        var service = new Mock<IUserService>();
        service.Setup(x => x.DeactivateAsync("user-1")).ReturnsAsync(true);

        var result = await new UsersController(service.Object).DeactivateUser("user-1");

        Assert.IsType<NoContentResult>(result);
    }
}