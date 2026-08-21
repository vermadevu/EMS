using API.Controllers;
using API.Interfaces.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace API.Tests.Controllers;

public class UploadControllerTests
{
    [Fact]
    public async Task UploadImage_ShouldReturnUploadedImageData()
    {
        var service = new Mock<ICloudinaryService>();
        var file = new FormFile(new MemoryStream(new byte[] { 1 }), 0, 1, "file", "image.png");
        service.Setup(x => x.UploadImageAsync(file)).ReturnsAsync(("images/1", "https://cdn.test/1"));

        var result = await new UploadController(service.Object).UploadImage(file);

        var response = Assert.IsType<OkObjectResult>(result.Result);
        var value = Assert.IsType<API.DTOs.ImageUploadDto>(response.Value);
        Assert.Equal("images/1", value.PublicId);
        Assert.Equal("https://cdn.test/1", value.Url);
    }
}