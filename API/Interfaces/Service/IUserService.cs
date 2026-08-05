using API.DTOs.Employee;
using API.DTOs.User;
using API.Helpers.Pagination;

namespace API.Interfaces.Service;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAllAsync();
    Task<UserDto?> GetByIdAsync(string id);
    Task<CreateUserResponseDto> CreateAsync(CreateUserDto dto);
    Task<bool> UpdateAsync(string id, UpdateUserDto dto);
    Task<bool> UpdateRolesAsync(string id, UpdateUserRolesDto dto);
    Task<bool> ActivateAsync(string id);
    Task<bool> DeactivateAsync(string id);
    Task<bool> ResetPasswordAsync(string id, ResetPasswordDto dto);
    Task<IEnumerable<string>> GetRolesAsync();
    Task<IList<string>?> GetUserRolesAsync(string id);
    Task<List<AvailableEmployeeDto>> GetAvailableEmployeesAsync();
    Task<PagedResult<UserListItemDto>> GetPagedAsync(UserQueryParams query);
}