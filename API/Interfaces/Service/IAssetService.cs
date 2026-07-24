using API.DTOs.Asset;

namespace API.Interfaces.Service;

public interface IAssetService
{
    Task<IEnumerable<AssetDto>> GetAllAsync();
    Task<AssetDto?> GetByIdAsync(int id);
    Task<AssetDto> CreateAsync(CreateAssetDto dto);
    Task<bool> UpdateAsync(int id, UpdateAssetDto dto);
    Task<bool> DeleteAsync(int id);
    Task<bool> AssignAssetAsync(int assetId, AssignAssetDto dto);
    Task<bool> ReturnAssetAsync(int assetId);
}