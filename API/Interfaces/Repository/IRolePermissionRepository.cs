using API.DTOs.RolePermissionManagement;
using Microsoft.AspNetCore.Identity;

namespace API.Interfaces.Repository
{
    public interface IRolePermissionRepository
    {
        Task<IEnumerable<IdentityRole>> GetRolesAsync();
        Task<RolePermissionsDto> GetRolePermissionsAsync(string roleId);
        Task UpdateRolePermissionsAsync(string roleId, List<int> permissionIds);
        Task<bool> AreValidPermissionIdsAsync(List<int> permissionIds);
        Task<List<string>> GetUserIdsInRoleAsync(string roleId);
    }
}
