using API.DTOs.Auth;
using API.Interfaces.Service;
using API.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace API.Services
{
    public class CurrentUserService(IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager, IPermissionService permissionService) : ICurrentUserService
    {
        private readonly IPermissionService _permissionService = permissionService;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        public string UserId =>
            _httpContextAccessor.HttpContext?
                .User
                .FindFirst(ClaimTypes.NameIdentifier)?
                .Value ?? "";

        public int EmployeeId
        {
            get
            {
                var value = _httpContextAccessor.HttpContext?
                    .User
                    .FindFirst("EmployeeId")?
                    .Value;

                return int.Parse(value!);
            }
        }

        public bool IsAuthenticated =>
            _httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;

        public bool IsInRole(string role)
        {
            return _httpContextAccessor.HttpContext?.User.IsInRole(role) ?? false;
        }

        public async Task<ApplicationUser> GetCurrentUserAsync()
        {
            var userId = _userManager.GetUserId(_httpContextAccessor.HttpContext!.User);

            if (string.IsNullOrWhiteSpace(userId))
                throw new UnauthorizedAccessException();

            var user = await _userManager.Users
                .Include(u => u.Employee)
                .FirstOrDefaultAsync(u => u.Id == userId);

            return user ?? throw new UnauthorizedAccessException();
        }
    }
}

