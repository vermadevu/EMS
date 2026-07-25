using API.DTOs.RolePermissionManagement;

namespace API.Interfaces.Service
{
    public interface IRolePermissionService
    {
        Task<IEnumerable<RoleDto>> GetRolesAsync();
        Task<RolePermissionsDto> GetRolePermissionsAsync(string roleId);
        Task UpdateRolePermissionsAsync(string roleId, UpdateRolePermissionsDto dto);
    }
}
