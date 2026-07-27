using API.Authorization;
using API.DTOs.Dashboard;
using API.DTOs.Dashboard.Widgets;
using API.Interfaces.Providers;
using API.Interfaces.Repository;
using API.Models.Dashboard;
using API.Models.Enums;
using AutoMapper;

namespace API.Providers.Dashboard
{
    public class RecentEmployeesProvider(IEmployeeRepository employeeRepository, IMapper mapper) : IDashboardWidgetProvider
    {
        private readonly IMapper _mapper = mapper;
        private readonly IEmployeeRepository _employeeRepository = employeeRepository;
        public DashboardWidgetType WidgetType => DashboardWidgetType.RecentEmployees;

        public bool CanBuild(DashboardContext context)
        {
            return context.Permissions.Contains(Permissions.Employees.Read);
        }

        public async Task<DashboardWidgetDto> BuildAsync(DashboardContext context)
        {
            var employees = await _employeeRepository.GetRecentEmployeesAsync();

            return new DashboardWidgetDto
            {
                Type = WidgetType,
                Title = "Recent Employees",
                Order = 10,
                Width = 6,
                Data = new RecentEmployeesWidgetDto
                {
                    Employees = _mapper.Map<List<RecentEmployeeDto>>(employees)
                }
            };
        }
    }
}
