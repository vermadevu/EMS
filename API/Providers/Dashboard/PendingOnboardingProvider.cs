using API.Authorization;
using API.DTOs.Dashboard;
using API.DTOs.Dashboard.Widgets;
using API.Interfaces.Providers;
using API.Interfaces.Repository;
using API.Models.Dashboard;
using API.Models.Enums;
using AutoMapper;

namespace API.Providers.Dashboard;

public class PendingOnboardingProvider(
    IEmployeeRepository employeeRepository,
    IMapper mapper)
    : IDashboardWidgetProvider
{
    private readonly IEmployeeRepository _employeeRepository = employeeRepository;
    private readonly IMapper _mapper = mapper;

    public DashboardWidgetType WidgetType => DashboardWidgetType.PendingOnboarding;

    public bool CanBuild(DashboardContext context)
    {
        return context.Permissions.Contains(Permissions.Employees.Read);
    }

    public async Task<DashboardWidgetDto> BuildAsync(
        DashboardContext context)
    {
        var employees = await _employeeRepository.GetPendingOnboardingAsync();

        return new DashboardWidgetDto
        {
            Type = WidgetType,
            Title = "Pending Onboarding",
            Order = 11,
            Width = 6,
            Data = new PendingOnboardingWidgetDto
            {
                Employees = _mapper.Map<List<RecentEmployeeDto>>(employees)
            }
        };
    }
}