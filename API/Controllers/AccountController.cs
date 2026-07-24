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
                Roles = roles
            };
        }

        [HttpPost("create-user")]
        public async Task<IActionResult> Register(CreateUserDto userDto)
        {
            var employee = await _context.Employees.FindAsync(userDto.EmployeeId);

            if (employee == null) {
                return NotFound("Employee Not Found");
            }

            var existingUser = await _userManager.Users.FirstOrDefaultAsync(u => u.EmployeeId == userDto.EmployeeId);

            if (existingUser != null)
            {
                return BadRequest("Employee already has a login account.");
            }

            if (await _userManager.FindByEmailAsync(userDto.Email) != null)
            {
                return BadRequest("Email already exists.");
            }

            if (await _userManager.FindByNameAsync(userDto.UserName) != null)
            {
                return BadRequest("Username already exists.");
            }

            if(!await _roleManager.RoleExistsAsync(userDto.Role))
            {
                return BadRequest("Invalid Role");
            }

            var user = new ApplicationUser
            {
                EmployeeId = userDto.EmployeeId,
                UserName = userDto.UserName,
                Email = userDto.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, userDto.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            await _userManager.AddToRoleAsync(user, userDto.Role);

            return Ok("User account created successfully");
        }



    }
}
