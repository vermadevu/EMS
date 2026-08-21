using API.DTOs.Dashboard;
using API.Interfaces.Providers;
using API.Interfaces.Repository;
using API.Interfaces.Service;
using API.Models.Dashboard;
using API.Models.Identity;
using API.Models.Enums;
using API.Services;
using Moq;
using Xunit;

namespace API.Tests.Services;

public class DashboardServiceTests
{
    [Fact]
    public async Task GetDashboardAsync_ShouldIncludeAllowedWidgetsInOrder()
    {
        var currentUser = new Mock<ICurrentUserService>();
        currentUser.Setup(x => x.GetCurrentUserAsync()).ReturnsAsync(new ApplicationUser { Id = "user-1" });
        var permissions = new Mock<IPermissionRepository>();
        permissions.Setup(x => x.GetEffectivePermissionsAsync("user-1"))
            .ReturnsAsync(new HashSet<string> { "Dashboard.View" });
        var providers = new[]
        {
            CreateProvider(20, true, "Later"),
            CreateProvider(5, true, "First"),
            CreateProvider(10, false, "Skipped")
        };
        var service = new DashboardService(providers, currentUser.Object, permissions.Object);

        var result = await service.GetDashboardAsync();

        Assert.Equal(new[] { "First", "Later" }, result.Widgets.Select(x => x.Title));
        Assert.DoesNotContain(result.Widgets, widget => widget.Title == "Skipped");
    }

    private static IDashboardWidgetProvider CreateProvider(int order, bool canBuild, string title)
    {
        var provider = new Mock<IDashboardWidgetProvider>();
        provider.SetupGet(x => x.WidgetType).Returns(DashboardWidgetType.QuickActions);
        provider.Setup(x => x.CanBuild(It.IsAny<DashboardContext>())).Returns(canBuild);
        provider.Setup(x => x.BuildAsync(It.IsAny<DashboardContext>())).ReturnsAsync(new DashboardWidgetDto
        {
            Title = title,
            Order = order
        });
        return provider.Object;
    }
}