using API.Controllers;
using API.DTOs.Document;
using API.Interfaces.Service;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace API.Tests.Controllers;

public class DocumentControllerTests
{
    [Fact]
    public async Task GetDocument_ShouldReturn404_WhenDocumentDoesNotExist()
    {
        var service = new Mock<IDocumentService>();
        service.Setup(x => x.GetByIdAsync(404)).ReturnsAsync((DocumentDto?)null);

        var result = await new DocumentController(service.Object).GetDocument(404);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task DeleteDocument_ShouldReturn204_WhenDeleted()
    {
        var service = new Mock<IDocumentService>();
        service.Setup(x => x.DeleteAsync(1)).ReturnsAsync(true);

        var result = await new DocumentController(service.Object).DeleteDocument(1);

        Assert.IsType<NoContentResult>(result);
    }
}