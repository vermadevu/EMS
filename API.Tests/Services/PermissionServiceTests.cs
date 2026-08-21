using API.Interfaces.Repository;
using API.Mapping;
using API.Models.Authorization;
using API.Services;
using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Xunit;

namespace API.Tests.Services;

public class PermissionServiceTests
{
    private readonly Mock<IPermissionRepository> _repository = new();
    private readonly PermissionService _service;

    public PermissionServiceTests()
    {
        var mapper = new MapperConfiguration(configuration =>
            configuration.AddProfile<MappingProfile>()).CreateMapper();
        var cache = new MemoryCache(new MemoryCacheOptions());

        _service = new PermissionService(_repository.Object, cache, mapper);
    }

    [Fact]
    public async Task GetPermissionsAsync_ShouldLoadEffectivePermissions()
    {
        var permissions = new HashSet<string> { "Asset.Read", "Asset.ReadOwn" };
        _repository.Setup(repository => repository.GetEffectivePermissionsAsync("user-1"))
            .ReturnsAsync(permissions);

        var result = await _service.GetPermissionsAsync("user-1");

        Assert.Equal(permissions, result);
        _repository.Verify(repository => repository.GetEffectivePermissionsAsync("user-1"), Times.Once);
    }

    [Fact]
    public async Task GetPermissionsAsync_ShouldUseCachedPermissions()
    {
        var permissions = new HashSet<string> { "Department.Read" };
        _repository.Setup(repository => repository.GetEffectivePermissionsAsync("user-1"))
            .ReturnsAsync(permissions);

        await _service.GetPermissionsAsync("user-1");
        var result = await _service.GetPermissionsAsync("user-1");

        Assert.Contains("Department.Read", result);
        _repository.Verify(repository => repository.GetEffectivePermissionsAsync("user-1"), Times.Once);
    }

    [Fact]
    public async Task HasPermissionAsync_ShouldReturnTrueOnlyForEffectivePermission()
    {
        _repository.Setup(repository => repository.GetEffectivePermissionsAsync("user-1"))
            .ReturnsAsync(new HashSet<string> { "Asset.Read" });

        Assert.True(await _service.HasPermissionAsync("user-1", "Asset.Read"));
        Assert.False(await _service.HasPermissionAsync("user-1", "Asset.Delete"));
    }

    [Fact]
    public async Task RefreshPermissionsAsync_ShouldForceRepositoryReload()
    {
        _repository.SetupSequence(repository => repository.GetEffectivePermissionsAsync("user-1"))
            .ReturnsAsync(new HashSet<string> { "Asset.Read" })
            .ReturnsAsync(new HashSet<string> { "Asset.Delete" });

        await _service.GetPermissionsAsync("user-1");
        await _service.RefreshPermissionsAsync("user-1");
        var result = await _service.GetPermissionsAsync("user-1");

        Assert.Contains("Asset.Delete", result);
        Assert.DoesNotContain("Asset.Read", result);
        _repository.Verify(repository => repository.GetEffectivePermissionsAsync("user-1"), Times.Exactly(2));
    }

    [Fact]
    public async Task GetAllAsync_ShouldMapPermissionsToDtos()
    {
        _repository.Setup(repository => repository.GetAllAsync())
            .ReturnsAsync(new[]
            {
                new Permission
                {
                    Id = 1,
                    Name = "Asset.Read",
                    DisplayName = "Read assets",
                    Category = "Assets",
                    Description = "View assets"
                }
            });

        var result = (await _service.GetAllAsync()).Single();

        Assert.Equal("Asset.Read", result.Name);
        Assert.Equal("Read assets", result.DisplayName);
        Assert.Equal("Assets", result.Category);
    }
}