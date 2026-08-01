using API.Authorization;
using API.Interfaces.Repository;
using API.Models.Enums;
using API.Providers.Dashboard.Base;

namespace API.Providers.Dashboard
{
    public class AssetStatisticsProvider(IAssetRepository repository) : StatisticWidgetProviderBase
    {
        private readonly IAssetRepository _repository = repository;
        public override DashboardWidgetType WidgetType => DashboardWidgetType.AssetStatistics;

        protected override string Permission => Permissions.Assets.Read;
        protected override string Title => "Assets";
        protected override string Icon => "inventory_2";
        protected override int Order => 4;
        protected override Task<int> GetCountAsync() => _repository.GetCountAsync();
    }
}
