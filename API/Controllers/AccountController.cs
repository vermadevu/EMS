using API.Data;
using API.DTOs.Auth;
using API.Interfaces.Service;
using API.Models.Identity;
using Humanizer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ITokenService tokenService, ApplicationDbContext context, RoleManager<IdentityRole> rolemanager) : BaseApiController
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
        private readonly ITokenService _tokenService = tokenService;
        private readonly ApplicationDbContext _context = context;
        private readonly RoleManager<IdentityRole> _roleManager = rolemanager;

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null) {
                return Unauthorized("Invalid email or password.");
            }

            var employee = user.EmployeeId.HasValue
                ? await _context.Employees.FindAsync(user.EmployeeId.Value)
                : null;

            var result = await _signInManager.CheckPasswordSignInAsync(
                user,
                loginDto.Password,
                false
            );

            if (!result.Succeeded)
            {
                return Unauthorized("Invalid email or password.");
            }

            var roles = await _userManager.GetRolesAsync(user);

            var token = await _tokenService.CreateTokenAsync(user, roles);

            return new LoginResponseDto
            {
                Token = token,
                Email = loginDto.Email!,
                UserName = user.UserName!,
                EmployeeStatus = employee?.Status,
                Roles = roles
            };
        }
    }
}
