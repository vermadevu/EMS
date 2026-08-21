using API.Controllers;
using API.DTOs.RolePermissionManagement;
using API.Interfaces.Service;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace API.Tests.Controllers;

public class RolePermissionControllerTests
{
    [Fact]
    public async Task UpdateRolePermissions_ShouldReturn204()
    {
        var service = new Mock<IRolePermissionService>();
        service.Setup(x => x.UpdateRolePermissionsAsync("role-1", It.IsAny<UpdateRolePermissionsDto>()))
            .Returns(Task.CompletedTask);

        var result = await new RolePermissionController(service.Object)
            .UpdateRolePermissions("role-1", new UpdateRolePermissionsDto());

        Assert.IsType<NoContentResult>(result);
    }
}