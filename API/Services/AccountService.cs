using API.DTOs.Auth;
using API.Exceptions;
using API.Interfaces.Service;
using API.Models.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

public class AccountService(
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    ITokenService tokenService,
    IPermissionService permissionService,
    IHttpContextAccessor httpContextAccessor)
    : IAccountService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
    private readonly ITokenService _tokenService = tokenService;
    private readonly IPermissionService _permissionService = permissionService;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public async Task<LoginResponseDto> LoginAsync(LoginDto loginDto)
    {
        var user = await _userManager.Users
            .Include(u => u.Employee)
            .FirstOrDefaultAsync(u => u.Email == loginDto.Email) 
        ?? throw new UnauthorizedException("Invalid email or password.");


        if (!user.IsActive)
        {
            throw new UnauthorizedException("Your account has been deactivated.");
        }

        var result = await _signInManager.CheckPasswordSignInAsync(
            user,
            loginDto.Password,
            false);

        if (!result.Succeeded)
        {
            Console.WriteLine("Hello there!");
            throw new UnauthorizedException("Invalid email or password.");
        }

        var roles = await _userManager.GetRolesAsync(user);

        var token = await _tokenService.CreateTokenAsync(user, roles);

        return new LoginResponseDto
        {
            Token = token,
            Email = user.Email!,
            UserName = user.UserName!,
            EmployeeStatus = user.Employee.Status,
            Roles = roles
        };
    }

    public async Task<CurrentUserDto> GetCurrentUserAsync()
    {
        var userId = _userManager.GetUserId(_httpContextAccessor.HttpContext!.User);

        if (string.IsNullOrWhiteSpace(userId))
        {
            throw new UnauthorizedException("Invalid User ID");
        }

        var user = await _userManager.Users
            .Include(u => u.Employee)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
            throw new UnauthorizedException("No User Found");
        }

        var roles = await _userManager.GetRolesAsync(user);

        var permissions = await _permissionService.GetPermissionsAsync(user.Id);

        return new CurrentUserDto
        {
            Id = user.Id,
            EmployeeId = user.EmployeeId,
            DisplayName = user.DisplayName,
            Email = user.Email!,
            EmployeeStatus = user.Employee.Status,
            Roles = roles,
            Permissions = permissions
        };
    }
}