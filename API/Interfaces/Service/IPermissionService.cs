using API.DTOs.Permission;

namespace API.Interfaces.Service;

public interface IPermissionService
{
    Task<bool> HasPermissionAsync(string userId, string permission);
    Task<HashSet<string>> GetPermissionsAsync(string userId);
    Task RefreshPermissionsAsync(string userId);
    Task<IEnumerable<PermissionDto>> GetAllAsync();
}