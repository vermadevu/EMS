using API.DTOs.Dashboard;
using API.Models.Dashboard;
using API.Models.Enums;

namespace API.Interfaces.Providers;

public interface IDashboardWidgetProvider
{
    DashboardWidgetType WidgetType { get; }
    bool CanBuild(DashboardContext context);
    Task<DashboardWidgetDto> BuildAsync(DashboardContext context);
}