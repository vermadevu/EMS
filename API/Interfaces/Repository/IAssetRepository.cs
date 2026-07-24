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
    Task<bool> EmployeeExistsAsync(int employeeId);
    Task<Asset?> GetAvailableAssetAsync(int id);
}