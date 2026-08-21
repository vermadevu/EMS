using API.Controllers;
using API.DTOs.Employee;
using API.Interfaces.Service;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace API.Tests.Controllers;

public class EmployeeControllerTests
{
    [Fact]
    public async Task GetById_ShouldReturn404_WhenEmployeeDoesNotExist()
    {
        var service = new Mock<IEmployeeService>();
        service.Setup(x => x.GetByIdAsync(404)).ReturnsAsync((EmployeeDto?)null);

        var result = await new EmployeeController(service.Object, new Mock<ICurrentUserService>().Object).GetById(404);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task GetStatuses_ShouldReturnAllEmployeeStatuses()
    {
        var controller = new EmployeeController(
            new Mock<IEmployeeService>().Object,
            new Mock<ICurrentUserService>().Object);

        var result = controller.GetStatuses();

        var response = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(5, Assert.IsAssignableFrom<IEnumerable<object>>(response.Value).Count());
    }
}