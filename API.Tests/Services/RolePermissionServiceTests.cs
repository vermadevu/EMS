using API.DTOs.RolePermissionManagement;
using API.Exceptions;
using API.Interfaces.Repository;
using API.Interfaces.Service;
using API.Services;
using Microsoft.AspNetCore.Identity;
using Moq;
using Xunit;

namespace API.Tests.Services;

public class RolePermissionServiceTests
{
    [Fact]
    public async Task GetRolesAsync_ShouldMapIdentityRoles()
    {
        var repository = new Mock<IRolePermissionRepository>();
        repository.Setup(x => x.GetRolesAsync()).ReturnsAsync(new[]
        {
            new IdentityRole { Id = "role-1", Name = "Admin" }
        });
        var service = new RolePermissionService(repository.Object, new Mock<IPermissionService>().Object);

        var result = (await service.GetRolesAsync()).Single();

        Assert.Equal("role-1", result.Id);
        Assert.Equal("Admin", result.Name);
    }

    [Fact]
    public async Task UpdateRolePermissionsAsync_ShouldRejectEmptyPermissions()
    {
        var service = CreateService(out _, out _);

        await Assert.ThrowsAsync<BadRequestException>(() => service.UpdateRolePermissionsAsync(
            "role-1", new UpdateRolePermissionsDto()));
    }

    [Fact]
    public async Task UpdateRolePermissionsAsync_ShouldRejectInvalidPermissionIds()
    {
        var service = CreateService(out var repository, out _);
        repository.Setup(x => x.AreValidPermissionIdsAsync(new List<int> { 99 })).ReturnsAsync(false);

        await Assert.ThrowsAsync<BadRequestException>(() => service.UpdateRolePermissionsAsync(
            "role-1", new UpdateRolePermissionsDto { PermissionIds = [99] }));

        repository.Verify(x => x.UpdateRolePermissionsAsync(It.IsAny<string>(), It.IsAny<List<int>>()), Times.Never);
    }

    [Fact]
    public async Task UpdateRolePermissionsAsync_ShouldUpdateAndRefreshAllUsersInRole()
    {
        var service = CreateService(out var repository, out var permissionService);
        var ids = new List<int> { 1, 2 };
        repository.Setup(x => x.AreValidPermissionIdsAsync(ids)).ReturnsAsync(true);
        repository.Setup(x => x.GetUserIdsInRoleAsync("role-1"))
            .ReturnsAsync(["user-1", "user-2"]);

        await service.UpdateRolePermissionsAsync("role-1", new UpdateRolePermissionsDto { PermissionIds = ids });

        repository.Verify(x => x.UpdateRolePermissionsAsync("role-1", ids), Times.Once);
        permissionService.Verify(x => x.RefreshPermissionsAsync("user-1"), Times.Once);
        permissionService.Verify(x => x.RefreshPermissionsAsync("user-2"), Times.Once);
    }

    private static RolePermissionService CreateService(
        out Mock<IRolePermissionRepository> repository,
        out Mock<IPermissionService> permissionService)
    {
        repository = new Mock<IRolePermissionRepository>();
        permissionService = new Mock<IPermissionService>();
        return new RolePermissionService(repository.Object, permissionService.Object);
    }
}