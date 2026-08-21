using API.DTOs.UserPermissionManagement;
using API.Exceptions;
using API.Interfaces.Repository;
using API.Interfaces.Service;
using API.Services;
using Moq;
using Xunit;

namespace API.Tests.Services;

public class UserPermissionServiceTests
{
    [Fact]
    public async Task UpdateUserPermissionsAsync_ShouldRejectInvalidPermissionIds()
    {
        var repository = new Mock<IUserPermissionRepository>();
        repository.Setup(x => x.AreValidPermissionIdsAsync(new List<int> { 1, 2 })).ReturnsAsync(false);
        var service = new UserPermissionService(repository.Object, new Mock<IPermissionService>().Object);

        await Assert.ThrowsAsync<BadRequestException>(() => service.UpdateUserPermissionsAsync(
            "user-1", new UpdateUserPermissionsDto
            {
                Permissions =
                [
                    new UserPermissionOverrideDto { PermissionId = 1 },
                    new UserPermissionOverrideDto { PermissionId = 2 }
                ]
            }));

        repository.Verify(x => x.UpdateUserPermissionsAsync(It.IsAny<string>(), It.IsAny<List<UserPermissionOverrideDto>>()), Times.Never);
    }

    [Fact]
    public async Task UpdateUserPermissionsAsync_ShouldDistinctIdsUpdateAndRefreshCache()
    {
        var repository = new Mock<IUserPermissionRepository>();
        var permissionService = new Mock<IPermissionService>();
        repository.Setup(x => x.AreValidPermissionIdsAsync(new List<int> { 1, 2 })).ReturnsAsync(true);
        var permissions = new List<UserPermissionOverrideDto>
        {
            new() { PermissionId = 1, IsAllowed = true },
            new() { PermissionId = 1, IsAllowed = false },
            new() { PermissionId = 2, IsAllowed = true }
        };
        var service = new UserPermissionService(repository.Object, permissionService.Object);

        await service.UpdateUserPermissionsAsync("user-1", new UpdateUserPermissionsDto { Permissions = permissions });

        repository.Verify(x => x.AreValidPermissionIdsAsync(new List<int> { 1, 2 }), Times.Once);
        repository.Verify(x => x.UpdateUserPermissionsAsync("user-1", permissions), Times.Once);
        permissionService.Verify(x => x.RefreshPermissionsAsync("user-1"), Times.Once);
    }
}