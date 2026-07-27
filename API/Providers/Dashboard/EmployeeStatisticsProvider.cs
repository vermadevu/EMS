using API.Authorization;
using API.Interfaces.Repository;
using API.Models.Enums;
using API.Providers.Dashboard.Base;

namespace API.Providers.Dashboard;

public class EmployeeStatisticsProvider(IEmployeeRepository employeeRepository) : StatisticWidgetProviderBase
{
    private readonly IEmployeeRepository _employeeRepository = employeeRepository;

    public override DashboardWidgetType WidgetType => DashboardWidgetType.EmployeeStatistics;
    protected override string Permission => Permissions.Employees.Read;
    protected override string Title => "Employees";
    protected override string Icon => "badge";
    protected override int Order => 1;

    protected override Task<int> GetCountAsync()
    {
        return _employeeRepository.CountAsync();
    }
}