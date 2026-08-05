using API.Authorization;
using API.DTOs.Dashboard;
using API.DTOs.Dashboard.Widgets;
using API.Interfaces.Providers;
using API.Interfaces.Repository;
using API.Models.Dashboard;
using API.Models.Enums;
using AutoMapper;

namespace API.Providers.Dashboard;

public class PendingApprovalProvider(
    IEmployeeRepository employeeRepository,
    IMapper mapper)
    : IDashboardWidgetProvider
{
    private readonly IEmployeeRepository _employeeRepository = employeeRepository;
    private readonly IMapper _mapper = mapper;

    public DashboardWidgetType WidgetType =>
        DashboardWidgetType.PendingApproval;

    public bool CanBuild(DashboardContext context)
    {
        return context.Permissions.Contains(
            Permissions.Employees.Read
        );
    }

    public async Task<DashboardWidgetDto> BuildAsync(
        DashboardContext context)
    {
        var employees =
            await _employeeRepository.GetPendingApprovalAsync();

        return new DashboardWidgetDto
        {
            Type = WidgetType,
            Title = "Pending Approval",
            Order = 8,
            Width = 6,
            Data = new RecentEmployeesWidgetDto
            {
                Employees =
                    _mapper.Map<List<RecentEmployeeDto>>(employees)
            }
        };
    }
}