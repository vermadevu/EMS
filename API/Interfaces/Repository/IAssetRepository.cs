using API.DTOs.Asset;
using API.Helpers.Pagination;
using API.Models.Entities;

namespace API.Interfaces.Repository;

public interface IAssetRepository
{
    Task<IEnumerable<Asset>> GetAllAsync();
    Task<Asset?> GetByIdAsync(int id);
    Task AddAsync(Asset asset);
    Task UpdateAsync(Asset asset);
    Task DeleteAsync(Asset asset);
    Task<bool> ExistsBySerialNumberAsync(string serialNumber);
    Task<bool> ExistsBySerialNumberAsync(string serialNumber, int excludeId);
    Task<string?> GetLastAssetCodeAsync();
    Task<Asset?> GetAvailableAssetAsync(int id);
    Task<int> GetCountAsync();
    Task<PagedResult<AssetListItemDto>> GetPagedAsync(AssetQueryParams queryParams);
    Task<IEnumerable<Asset>> GetByEmployeeAsync(int employeeId);

}