using API.DTOs.Auth;
using API.Exceptions;
using API.Interfaces.Repository;
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
    IRefreshTokenRepository refreshTokenRepository,
    IHttpContextAccessor httpContextAccessor,
    IConfiguration configuration)
    : IAccountService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
    private readonly ITokenService _tokenService = tokenService;
    private readonly IPermissionService _permissionService = permissionService;
    private readonly IRefreshTokenRepository _refreshTokenRepository = refreshTokenRepository;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly IConfiguration _configuration = configuration;

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

        var accessToken = await _tokenService.CreateTokenAsync(user, roles);

        var refreshToken = _tokenService.GenerateRefreshToken();

        var existingRefreshToken = await _refreshTokenRepository.GetByUserIdAsync(user.Id);

        if (existingRefreshToken is null)
        {
            await _refreshTokenRepository.AddAsync(new RefreshToken
            {
                UserId = user.Id,
                Token = refreshToken,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(
                    Convert.ToDouble(_configuration.GetValue<int>("Jwt:RefreshTokenExpiryInDays"))
                )
            });
        }
        else
        {
            existingRefreshToken.Token = refreshToken;
            existingRefreshToken.CreatedAt = DateTime.UtcNow;
            existingRefreshToken.ExpiresAt = DateTime.UtcNow.AddDays(
                                     Convert.ToDouble(_configuration.GetValue<int>("Jwt:RefreshTokenExpiryInDays")));

            await _refreshTokenRepository.UpdateAsync(existingRefreshToken);
        }

        return new LoginResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
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
            Permissions = permissions,
            ProfileImage = user.Employee?.ProfileImage
        };
    }

    public async Task<RefreshResponseDto> RefreshAsync(RefreshRequestDto refreshDto)
    {
        var storedToken = await _refreshTokenRepository.GetByTokenAsync(refreshDto.RefreshToken);

        if (storedToken == null)
        {
            throw new UnauthorizedException("Invalid refresh token.");
        }

        if (storedToken.ExpiresAt <= DateTime.UtcNow)
        {
            throw new UnauthorizedException("Refresh token expired.");
        }

        var user = storedToken.User;

        if (!user.IsActive)
        {
            throw new UnauthorizedException("User account is inactive.");
        }

        var roles = await _userManager.GetRolesAsync(user);
        var accessToken = await _tokenService.CreateTokenAsync(user, roles);
        var refreshToken = _tokenService.GenerateRefreshToken();

        storedToken.Token = refreshToken;
        storedToken.CreatedAt = DateTime.UtcNow;
        storedToken.ExpiresAt = DateTime.UtcNow.AddDays(
                        Convert.ToDouble(_configuration.GetValue<int>("Jwt:RefreshTokenExpiryInDays")));

        await _refreshTokenRepository.UpdateAsync(storedToken);

        return new RefreshResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }
}