using API.DTOs.Department;
using API.Exceptions;
using API.Interfaces.Repository;
using API.Mapping;
using API.Models.Entities;
using API.Services;
using AutoMapper;
using Moq;
using Xunit;

namespace API.Tests.Services;

public class DepartmentServiceTests
{
    private readonly Mock<IDepartmentRepository> _repository = new();
    private readonly DepartmentService _service;

    public DepartmentServiceTests()
    {
        var mapper = new MapperConfiguration(configuration =>
            configuration.AddProfile<MappingProfile>()).CreateMapper();

        _service = new DepartmentService(_repository.Object, mapper);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnMappedDepartments()
    {
        _repository
            .Setup(repository => repository.GetAllAsync())
            .ReturnsAsync(new[]
            {
                new Department { Id = 1, Name = "Engineering", Description = "Product development" },
                new Department { Id = 2, Name = "Finance" }
            });

        var result = (await _service.GetAllAsync()).ToList();

        Assert.Equal(2, result.Count);
        Assert.Equal("Engineering", result[0].Name);
        Assert.Equal("Product development", result[0].Description);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenDepartmentDoesNotExist()
    {
        _repository
            .Setup(repository => repository.GetByIdAsync(42))
            .ReturnsAsync((Department?)null);

        var result = await _service.GetByIdAsync(42);

        Assert.Null(result);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowAndNotAdd_WhenNameAlreadyExists()
    {
        var request = new CreateDepartmentDto { Name = "Engineering" };
        _repository
            .Setup(repository => repository.ExistsByNameAsync(request.Name))
            .ReturnsAsync(true);

        await Assert.ThrowsAsync<BadRequestException>(() => _service.CreateAsync(request));

        _repository.Verify(repository => repository.AddAsync(It.IsAny<Department>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_ShouldMapAddAndReturnDepartment()
    {
        var request = new CreateDepartmentDto
        {
            Name = "Engineering",
            Description = "Product development"
        };
        _repository
            .Setup(repository => repository.ExistsByNameAsync(request.Name))
            .ReturnsAsync(false);
        _repository
            .Setup(repository => repository.AddAsync(It.IsAny<Department>()))
            .Returns(Task.CompletedTask);

        var result = await _service.CreateAsync(request);

        Assert.Equal(request.Name, result.Name);
        Assert.Equal(request.Description, result.Description);
        _repository.Verify(repository => repository.AddAsync(It.Is<Department>(department =>
            department.Name == request.Name && department.Description == request.Description)), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnFalse_WhenDepartmentDoesNotExist()
    {
        _repository
            .Setup(repository => repository.GetByIdAsync(42))
            .ReturnsAsync((Department?)null);

        var result = await _service.UpdateAsync(42, new UpdateDepartmentDto { Name = "Updated" });

        Assert.False(result);
        _repository.Verify(repository => repository.UpdateAsync(It.IsAny<Department>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteExistingDepartment()
    {
        var department = new Department { Id = 7, Name = "Finance" };
        _repository
            .Setup(repository => repository.GetByIdAsync(department.Id))
            .ReturnsAsync(department);
        _repository
            .Setup(repository => repository.DeleteAsync(department))
            .Returns(Task.CompletedTask);

        var result = await _service.DeleteAsync(department.Id);

        Assert.True(result);
        _repository.Verify(repository => repository.DeleteAsync(department), Times.Once);
    }
}