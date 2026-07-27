using API.Authorization;
using API.Interfaces.Repository;
using API.Models.Enums;
using API.Providers.Dashboard.Base;

namespace API.Providers.Dashboard
{
    public class DepartmentStatisticsProvider(IDepartmentRepository repository) : StatisticWidgetProviderBase
    {
        private readonly IDepartmentRepository _repository = repository;

        public override DashboardWidgetType WidgetType => DashboardWidgetType.DepartmentStatistics;
        protected override string Permission => Permissions.Departments.Read;
        protected override string Title => "Departments";
        protected override string Icon => "apartment";
        protected override int Order => 2;
        protected override Task<int> GetCountAsync()
            => _repository.CountAsync();
    }
}
