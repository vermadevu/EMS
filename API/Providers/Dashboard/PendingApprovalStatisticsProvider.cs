using API.Authorization;
using API.Data;
using API.Models.Enums;
using API.Providers.Dashboard.Base;
using Microsoft.EntityFrameworkCore;

namespace API.Providers.Dashboard;

public class PendingApprovalStatisticsProvider(ApplicationDbContext context)
    : StatisticWidgetProviderBase
{
    private readonly ApplicationDbContext _context = context;

    public override DashboardWidgetType WidgetType => DashboardWidgetType.PendingApprovalStatistics;

    protected override string Permission => Permissions.Employees.Read;

    protected override string Title => "Pending Approval";

    protected override string Icon => "pending_actions";

    protected override int Order => 2;

    protected override async Task<int> GetCountAsync()
    {
        return await _context.Employees.CountAsync(x =>
            x.Status == EmployeeStatus.DocumentsSubmitted);
    }
}