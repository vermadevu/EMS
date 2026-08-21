using API.Controllers;
using API.DTOs.Designation;
using API.Interfaces.Service;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace API.Tests.Controllers;

public class DesignationControllerTests
{
    [Fact]
    public async Task GetById_ShouldReturn404_WhenMissing()
    {
        var service = new Mock<IDesignationService>();
        service.Setup(x => x.GetByIdAsync(99)).ReturnsAsync((DesignationDto?)null);

        var result = await new DesignationController(service.Object).GetById(99);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task Create_ShouldReturn201()
    {
        var service = new Mock<IDesignationService>();
        var expected = new DesignationDto { Id = 1, Name = "Developer" };
        service.Setup(x => x.CreateAsync(It.IsAny<CreateDesignationDto>())).ReturnsAsync(expected);

        var result = await new DesignationController(service.Object).Create(new CreateDesignationDto());

        var response = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Same(expected, response.Value);
    }
}