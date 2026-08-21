using API.Controllers;
using API.Interfaces.Service;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace API.Tests.Controllers;

public class PermissionControllerTests
{
    [Fact]
    public async Task GetAll_ShouldReturn200WithPermissions()
    {
        var service = new Mock<IPermissionService>();
        var expected = Array.Empty<API.DTOs.Permission.PermissionDto>();
        service.Setup(x => x.GetAllAsync()).ReturnsAsync(expected);

        var result = await new PermissionController(service.Object).GetAll();

        Assert.Same(expected, Assert.IsType<OkObjectResult>(result.Result).Value);
    }
}