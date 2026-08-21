using API.Controllers;
using API.DTOs.Dashboard;
using API.Interfaces.Service;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace API.Tests.Controllers;

public class DashboardControllerTests
{
    [Fact]
    public async Task GetDashboard_ShouldReturn200WithDashboard()
    {
        var service = new Mock<IDashboardService>();
        var expected = new DashboardDto();
        service.Setup(x => x.GetDashboardAsync()).ReturnsAsync(expected);

        var result = await new DashboardController(service.Object).GetDashboard();

        Assert.Same(expected, Assert.IsType<OkObjectResult>(result.Result).Value);
    }
}