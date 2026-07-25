using API.DTOs.User;

namespace API.Interfaces.Service;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAllAsync();
    Task<UserDto?> GetByIdAsync(string id);
    Task<UserDto> CreateAsync(CreateUserDto dto);
    Task<bool> UpdateAsync(string id, UpdateUserDto dto);
    Task<bool> UpdateRolesAsync(string id, UpdateUserRolesDto dto);
    Task<bool> ActivateAsync(string id);
    Task<bool> DeactivateAsync(string id);
    Task<bool> ResetPasswordAsync(string id, ResetPasswordDto dto);
    Task<IEnumerable<string>> GetRolesAsync();
    Task<IList<string>?> GetUserRolesAsync(string id);
}