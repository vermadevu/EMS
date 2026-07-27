using API.DTOs.Dashboard;
using API.DTOs.Dashboard.Widgets;
using API.Interfaces.Providers;
using API.Models.Dashboard;
using API.Models.Enums;

namespace API.Providers.Dashboard.Base;

public abstract class StatisticWidgetProviderBase
    : IDashboardWidgetProvider
{
    public abstract DashboardWidgetType WidgetType { get; }
    protected abstract string Permission { get; }
    protected abstract string Title { get; }
    protected abstract string Icon { get; }
    protected abstract int Order { get; }
    protected virtual int Width => 3;
    protected abstract Task<int> GetCountAsync();

    public bool CanBuild(DashboardContext context)
    {
        return context.Permissions.Contains(Permission);
    }

    public async Task<DashboardWidgetDto> BuildAsync(
        DashboardContext context)
    {
        var count = await GetCountAsync();
        return new DashboardWidgetDto
        {
            Type = WidgetType,
            Title = Title,
            Order = Order,
            Width = Width,
            Data = new StatisticWidgetDto
            {
                Count = count,
                Icon = Icon
            }
        };
    }
}