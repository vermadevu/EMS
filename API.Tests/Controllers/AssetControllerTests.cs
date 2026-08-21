using API.Controllers;
using API.DTOs.Asset;
using API.Interfaces.Service;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace API.Tests.Controllers;

public class AssetControllerTests
{
    [Fact]
    public async Task GetAsset_ShouldReturn404_WhenAssetDoesNotExist()
    {
        var service = new Mock<IAssetService>();
        service.Setup(x => x.GetByIdAsync(404)).ReturnsAsync((AssetDto)null!);

        var result = await new AssetController(service.Object).GetAsset(404);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task DeleteAsset_ShouldReturn204()
    {
        var service = new Mock<IAssetService>();
        service.Setup(x => x.DeleteAsync(1)).Returns(Task.CompletedTask);

        var result = await new AssetController(service.Object).DeleteAsset(1);

        Assert.IsType<NoContentResult>(result);
    }
}