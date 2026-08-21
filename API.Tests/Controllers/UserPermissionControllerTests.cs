using API.Controllers;
using API.DTOs.UserPermissionManagement;
using API.Interfaces.Service;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace API.Tests.Controllers;

public class UserPermissionControllerTests
{
    [Fact]
    public async Task UpdateUserPermissions_ShouldReturn204()
    {
        var service = new Mock<IUserPermissionService>();
        service.Setup(x => x.UpdateUserPermissionsAsync("user-1", It.IsAny<UpdateUserPermissionsDto>()))
            .Returns(Task.CompletedTask);

        var result = await new UserPermissionController(service.Object)
            .UpdateUserPermissions("user-1", new UpdateUserPermissionsDto());

        Assert.IsType<NoContentResult>(result);
    }
}