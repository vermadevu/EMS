using API.DTOs.Asset;
using API.Helpers.Pagination;

namespace API.Interfaces.Service;

public interface IAssetService
{
    Task<IEnumerable<AssetDto>> GetAllAsync();
    Task<AssetDto> GetByIdAsync(int id);
    Task<AssetDto> CreateAsync(CreateAssetDto dto);
    Task<AssetDto> UpdateAsync(int id, UpdateAssetDto dto);
    Task DeleteAsync(int id);
    Task AssignAsync(int assetId, AssignAssetDto dto);
    Task ReturnAsync(int assetId);
    Task<PagedResult<AssetListItemDto>> GetPagedAsync(AssetQueryParams queryParams);
    Task<IEnumerable<AssetDto>> GetByEmployeeAsync(int employeeId);
    Task<IEnumerable<AssetDto>> GetMyAssetsAsync();

}