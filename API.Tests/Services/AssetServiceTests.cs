using API.DTOs.Asset;
using API.Exceptions;
using API.Interfaces.Repository;
using API.Interfaces.Service;
using API.Mapping;
using API.Models.Entities;
using API.Models.Enums;
using API.Services;
using AutoMapper;
using Moq;
using Xunit;

namespace API.Tests.Services;

public class AssetServiceTests
{
    private readonly Mock<IAssetRepository> _repository = new();
    private readonly Mock<IEmployeeRepository> _employeeRepository = new();
    private readonly Mock<ICurrentUserService> _currentUser = new();
    private readonly AssetService _service;

    public AssetServiceTests()
    {
        var mapper = new MapperConfiguration(configuration =>
            configuration.AddProfile<MappingProfile>()).CreateMapper();

        _service = new AssetService(
            _repository.Object,
            mapper,
            _employeeRepository.Object,
            _currentUser.Object);
    }

    [Fact]
    public async Task CreateAsync_ShouldGenerateCodeAndSetAvailable()
    {
        var request = new CreateAssetDto
        {
            AssetName = "Laptop",
            AssetType = AssetType.Laptop,
            SerialNumber = "SN-100"
        };
        _repository.Setup(repository => repository.ExistsBySerialNumberAsync(request.SerialNumber!)).ReturnsAsync(false);
        _repository.Setup(repository => repository.GetLastAssetCodeAsync()).ReturnsAsync("AST0009");

        Asset? addedAsset = null;
        _repository.Setup(repository => repository.AddAsync(It.IsAny<Asset>()))
            .Callback<Asset>(asset => addedAsset = asset)
            .Returns(Task.CompletedTask);

        var result = await _service.CreateAsync(request);

        Assert.NotNull(addedAsset);
        Assert.Equal("AST0010", addedAsset!.AssetCode);
        Assert.Equal(AssetStatus.Available, addedAsset.Status);
        Assert.Null(addedAsset.EmployeeId);
        Assert.Equal(request.AssetName, result.AssetName);
    }

    [Fact]
    public async Task CreateAsync_ShouldRejectDuplicateSerialNumber()
    {
        var request = new CreateAssetDto
        {
            AssetName = "Laptop",
            AssetType = AssetType.Laptop,
            SerialNumber = "SN-100"
        };
        _repository.Setup(repository => repository.ExistsBySerialNumberAsync(request.SerialNumber!)).ReturnsAsync(true);

        await Assert.ThrowsAsync<BadRequestException>(() => _service.CreateAsync(request));

        _repository.Verify(repository => repository.AddAsync(It.IsAny<Asset>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRejectAssignedAsset()
    {
        var asset = CreateAsset(1, AssetStatus.Assigned);
        _repository.Setup(repository => repository.GetByIdAsync(asset.Id)).ReturnsAsync(asset);

        await Assert.ThrowsAsync<BadRequestException>(() => _service.DeleteAsync(asset.Id));

        _repository.Verify(repository => repository.DeleteAsync(It.IsAny<Asset>()), Times.Never);
    }

    [Fact]
    public async Task AssignAsync_ShouldAssignAvailableAssetToActiveEmployee()
    {
        var asset = CreateAsset(1, AssetStatus.Available);
        var employee = CreateEmployee(7, EmployeeStatus.Active);
        _repository.Setup(repository => repository.GetAvailableAssetAsync(asset.Id)).ReturnsAsync(asset);
        _employeeRepository.Setup(repository => repository.GetByIdAsync(employee.Id)).ReturnsAsync(employee);

        await _service.AssignAsync(asset.Id, new AssignAssetDto { EmployeeId = employee.Id });

        Assert.Equal(employee.Id, asset.EmployeeId);
        Assert.Equal(AssetStatus.Assigned, asset.Status);
        _repository.Verify(repository => repository.UpdateAsync(asset), Times.Once);
    }

    [Fact]
    public async Task AssignAsync_ShouldRejectInactiveEmployee()
    {
        var asset = CreateAsset(1, AssetStatus.Available);
        var employee = CreateEmployee(7, EmployeeStatus.Inactive);
        _repository.Setup(repository => repository.GetAvailableAssetAsync(asset.Id)).ReturnsAsync(asset);
        _employeeRepository.Setup(repository => repository.GetByIdAsync(employee.Id)).ReturnsAsync(employee);

        await Assert.ThrowsAsync<BadRequestException>(() => _service.AssignAsync(
            asset.Id,
            new AssignAssetDto { EmployeeId = employee.Id }));

        _repository.Verify(repository => repository.UpdateAsync(It.IsAny<Asset>()), Times.Never);
    }

    [Fact]
    public async Task AssignAsync_ShouldRejectUnavailableAsset()
    {
        _repository.Setup(repository => repository.GetAvailableAssetAsync(1)).ReturnsAsync((Asset?)null);

        await Assert.ThrowsAsync<BadRequestException>(() => _service.AssignAsync(
            1,
            new AssignAssetDto { EmployeeId = 7 }));

        _employeeRepository.Verify(repository => repository.GetByIdAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task ReturnAsync_ShouldMakeAssignedAssetAvailable()
    {
        var asset = CreateAsset(1, AssetStatus.Assigned);
        asset.EmployeeId = 7;
        _repository.Setup(repository => repository.GetByIdAsync(asset.Id)).ReturnsAsync(asset);

        await _service.ReturnAsync(asset.Id);

        Assert.Equal(AssetStatus.Available, asset.Status);
        Assert.Null(asset.EmployeeId);
        _repository.Verify(repository => repository.UpdateAsync(asset), Times.Once);
    }

    [Fact]
    public async Task ReturnAsync_ShouldRejectAlreadyAvailableAsset()
    {
        var asset = CreateAsset(1, AssetStatus.Available);
        _repository.Setup(repository => repository.GetByIdAsync(asset.Id)).ReturnsAsync(asset);

        await Assert.ThrowsAsync<BadRequestException>(() => _service.ReturnAsync(asset.Id));

        _repository.Verify(repository => repository.UpdateAsync(It.IsAny<Asset>()), Times.Never);
    }

    private static Asset CreateAsset(int id, AssetStatus status)
    {
        return new Asset
        {
            Id = id,
            AssetCode = $"AST{id:D4}",
            AssetName = "Laptop",
            AssetType = AssetType.Laptop,
            Status = status
        };
    }

    private static Employee CreateEmployee(int id, EmployeeStatus status)
    {
        return new Employee
        {
            Id = id,
            EmployeeCode = $"E{id:D4}",
            FirstName = "Test",
            LastName = "Employee",
            Email = "employee@dems.test",
            Status = status
        };
    }
}