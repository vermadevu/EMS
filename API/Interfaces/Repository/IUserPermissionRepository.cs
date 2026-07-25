using API.DTOs.UserPermissionManagement;

namespace API.Interfaces.Repository;

public interface IUserPermissionRepository
{
    Task<IEnumerable<UserListDto>> GetUsersAsync();
    Task<UserPermissionsDto> GetUserPermissionsAsync(string userId);
    Task UpdateUserPermissionsAsync(string userId, List<UserPermissionOverrideDto> permissions);
    Task<bool> AreValidPermissionIdsAsync(List<int> permissionIds);
}