using API.DTOs.Dashboard;
using API.Interfaces.Providers;
using API.Interfaces.Repository;
using API.Interfaces.Service;
using API.Models.Dashboard;
using API.Repositories;

namespace API.Services
{
    public class DashboardService(
        IEnumerable<IDashboardWidgetProvider> providers,
        ICurrentUserService currentUserService,
        IPermissionRepository permissionRepository)
        : IDashboardService
    {
        private readonly ICurrentUserService _currentUserService = currentUserService;
        private readonly IPermissionRepository _permissionRepository = permissionRepository;
        private readonly IEnumerable<IDashboardWidgetProvider> _providers = providers;
        public async Task<DashboardDto> GetDashboardAsync()
        {
            var user = await _currentUserService.GetCurrentUserAsync();

            var permissions = await _permissionRepository
                .GetEffectivePermissionsAsync(user.Id);

            var context = new DashboardContext
            {
                User = user,
                Permissions = permissions
            };

           var widgets = new List<DashboardWidgetDto>();

            foreach (var provider in _providers)
            {
                if (!provider.CanBuild(context))
                    continue;

                widgets.Add(await provider.BuildAsync(context));
            }

            return new DashboardDto
            {
                Widgets = widgets
                    .OrderBy(w => w.Order)
                    .ToList()
            };
        }
    }
}
