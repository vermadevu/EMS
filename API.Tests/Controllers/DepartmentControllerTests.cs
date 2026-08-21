using API.Controllers;
using API.DTOs.Department;
using API.Helpers.Pagination;
using API.Interfaces.Service;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace API.Tests.Controllers;

public class DepartmentControllerTests
{
    private readonly Mock<IDepartmentService> _service = new();
    private readonly DepartmentController _controller;

    public DepartmentControllerTests()
    {
        _controller = new DepartmentController(_service.Object);
    }

    [Fact]
    public async Task GetAll_ShouldReturn200WithDepartments()
    {
        var departments = new[]
        {
            new DepartmentDto { Id = 1, Name = "Engineering" }
        };
        _service.Setup(service => service.GetAllAsync()).ReturnsAsync(departments);

        var result = await _controller.GetAll();

        var response = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Same(departments, response.Value);
    }

    [Fact]
    public async Task GetById_ShouldReturn200WithDepartment_WhenDepartmentExists()
    {
        var department = new DepartmentDto { Id = 1, Name = "Engineering" };
        _service.Setup(service => service.GetByIdAsync(department.Id)).ReturnsAsync(department);

        var result = await _controller.GetById(department.Id);

        Assert.Same(department, result.Value);
    }

    [Fact]
    public async Task GetById_ShouldReturn404_WhenDepartmentDoesNotExist()
    {
        _service.Setup(service => service.GetByIdAsync(42)).ReturnsAsync((DepartmentDto?)null);

        var result = await _controller.GetById(42);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task Create_ShouldReturn201WithLocation()
    {
        var request = new CreateDepartmentDto { Name = "Engineering" };
        var department = new DepartmentDto { Id = 1, Name = request.Name };
        _service.Setup(service => service.CreateAsync(request)).ReturnsAsync(department);

        var result = await _controller.Create(request);

        var response = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(nameof(DepartmentController.GetById), response.ActionName);
        Assert.Equal(department.Id, response.RouteValues!["id"]);
        Assert.Same(department, response.Value);
    }

    [Fact]
    public async Task Update_ShouldReturn204_WhenDepartmentIsUpdated()
    {
        var request = new UpdateDepartmentDto { Name = "Engineering" };
        _service.Setup(service => service.UpdateAsync(1, request)).ReturnsAsync(true);

        var result = await _controller.Update(1, request);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Update_ShouldReturn404_WhenDepartmentDoesNotExist()
    {
        var request = new UpdateDepartmentDto { Name = "Engineering" };
        _service.Setup(service => service.UpdateAsync(42, request)).ReturnsAsync(false);

        var result = await _controller.Update(42, request);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Delete_ShouldReturn204_WhenDepartmentIsDeleted()
    {
        _service.Setup(service => service.DeleteAsync(1)).ReturnsAsync(true);

        var result = await _controller.Delete(1);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task GetDepartments_ShouldReturnPagedResult()
    {
        var query = new DepartmentQueryParams { PageNumber = 2, PageSize = 10 };
        var pagedResult = new PagedResult<DepartmentListItemDto>
        {
            PageNumber = query.PageNumber,
            PageSize = query.PageSize,
            TotalCount = 11,
            Items = new[] { new DepartmentListItemDto { Id = 1, Name = "Engineering" } }
        };
        _service.Setup(service => service.GetPagedAsync(query)).ReturnsAsync(pagedResult);

        var result = await _controller.GetDepartments(query);

        var response = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Same(pagedResult, response.Value);
    }
}