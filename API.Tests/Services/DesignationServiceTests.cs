using API.DTOs.Designation;
using API.Exceptions;
using API.Interfaces.Repository;
using API.Mapping;
using API.Models.Entities;
using API.Services;
using AutoMapper;
using Moq;
using Xunit;

namespace API.Tests.Services;

public class DesignationServiceTests
{
    private readonly Mock<IDesignationRepository> _repository = new();
    private readonly DesignationService _service;

    public DesignationServiceTests()
    {
        var mapper = new MapperConfiguration(configuration =>
            configuration.AddProfile<MappingProfile>()).CreateMapper();

        _service = new DesignationService(_repository.Object, mapper);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnMappedDesignations()
    {
        _repository.Setup(repository => repository.GetAllAsync())
            .ReturnsAsync(new[]
            {
                new Designation { Id = 1, Name = "Developer", Description = "Builds products" },
                new Designation { Id = 2, Name = "Manager" }
            });

        var result = (await _service.GetAllAsync()).ToList();

        Assert.Equal(2, result.Count);
        Assert.Equal("Developer", result[0].Name);
        Assert.Equal("Builds products", result[0].Description);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenDesignationDoesNotExist()
    {
        _repository.Setup(repository => repository.GetByIdAsync(404))
            .ReturnsAsync((Designation?)null);

        var result = await _service.GetByIdAsync(404);

        Assert.Null(result);
    }

    [Fact]
    public async Task CreateAsync_ShouldRejectDuplicateName()
    {
        var request = new CreateDesignationDto { Name = "Developer" };
        _repository.Setup(repository => repository.ExistsByNameAsync(request.Name)).ReturnsAsync(true);

        await Assert.ThrowsAsync<BadRequestException>(() => _service.CreateAsync(request));

        _repository.Verify(repository => repository.AddAsync(It.IsAny<Designation>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_ShouldMapAddAndReturnDesignation()
    {
        var request = new CreateDesignationDto
        {
            Name = "Developer",
            Description = "Builds products"
        };
        _repository.Setup(repository => repository.ExistsByNameAsync(request.Name)).ReturnsAsync(false);
        _repository.Setup(repository => repository.AddAsync(It.IsAny<Designation>()))
            .Returns(Task.CompletedTask);

        var result = await _service.CreateAsync(request);

        Assert.Equal(request.Name, result.Name);
        Assert.Equal(request.Description, result.Description);
        _repository.Verify(repository => repository.AddAsync(It.Is<Designation>(designation =>
            designation.Name == request.Name && designation.Description == request.Description)), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldMapAndPersistExistingDesignation()
    {
        var designation = new Designation { Id = 3, Name = "Developer", Description = "Old" };
        var request = new UpdateDesignationDto { Name = "Senior Developer", Description = "Updated" };
        _repository.Setup(repository => repository.GetByIdAsync(designation.Id)).ReturnsAsync(designation);

        var result = await _service.UpdateAsync(designation.Id, request);

        Assert.True(result);
        Assert.Equal(request.Name, designation.Name);
        Assert.Equal(request.Description, designation.Description);
        _repository.Verify(repository => repository.UpdateAsync(designation), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnFalse_WhenDesignationDoesNotExist()
    {
        _repository.Setup(repository => repository.GetByIdAsync(404))
            .ReturnsAsync((Designation?)null);

        var result = await _service.DeleteAsync(404);

        Assert.False(result);
        _repository.Verify(repository => repository.DeleteAsync(It.IsAny<Designation>()), Times.Never);
    }
}