using API.DTOs.UserPermissionManagement;

namespace API.Interfaces.Service;

public interface IUserPermissionService
{
    Task<IEnumerable<UserListDto>> GetUsersAsync();
    Task<UserPermissionsDto> GetUserPermissionsAsync(string userId);
    Task UpdateUserPermissionsAsync(string userId, UpdateUserPermissionsDto dto);
}