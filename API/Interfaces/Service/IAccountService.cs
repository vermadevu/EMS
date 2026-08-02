using API.DTOs.Asset;
using API.DTOs.Auth;

namespace API.Interfaces.Service;

public interface IAccountService
{
    Task<LoginResponseDto> LoginAsync(LoginDto dto);
    Task<CurrentUserDto> GetCurrentUserAsync();
}