using API.DTOs.User;
using API.Interfaces.Repository;
using API.Interfaces.Service;
using API.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public class UserService(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IEmployeeRepository employeeRepository) : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;
        private readonly IEmployeeRepository _employeeRepository = employeeRepository;
        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var users = await _userManager.Users
               .Include(u => u.Employee)
               .ToListAsync();

            var userDtos = new List<UserDto>();

            foreach (var user in users)
            {
                userDtos.Add(await MapToUserDtoAsync(user));
            }

            return userDtos;
        }

        public async Task<UserDto?> GetByIdAsync(string id)
        {
            var user = await _userManager.Users
                .Include(u => u.Employee)   
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null) return null;

            return await MapToUserDtoAsync(user);

        }

        public async Task<IEnumerable<string>> GetRolesAsync()
        {
            return await _roleManager.Roles
                .Where(r => r.Name != null)
                .Select(r => r.Name!)
                .ToListAsync();
        }

        public async Task<UserDto> CreateAsync(CreateUserDto dto)
        {
            var employee = await _employeeRepository.GetByIdAsync(dto.EmployeeId) ?? throw new Exception("Employee not found.");

            if (await _employeeRepository.HasUserAccountAsync(dto.EmployeeId))
            {
                throw new Exception("User account already exists for this employee.");
            }

            var existingUser = await _userManager.FindByEmailAsync(dto.Email);

            if (existingUser != null)
            {
                throw new Exception("Email is already in use.");
            }
            
            if (dto.Roles.Count <= 0)
            {
                throw new Exception("At least one role must be assigned.");
            }

            foreach (var role in dto.Roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    throw new Exception($"Role '{role}' does not exist.");
                }
            }

            var user = new ApplicationUser
            {
                EmployeeId = dto.EmployeeId,
                DisplayName = employee.FullName,
                UserName = dto.Email,
                Email = dto.Email,
                EmailConfirmed = true,
                IsActive = true
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                throw new Exception(string.Join(", ",
                    result.Errors.Select(e => e.Description)));
            }

            var roleResult = await _userManager.AddToRolesAsync(user, dto.Roles);

            if (!roleResult.Succeeded)
            {
                // Delete the user if something goes wrong if user created and roles couldn't assign then user gets deleted to ensure no user exists without a 
                await _userManager.DeleteAsync(user); 

                throw new Exception(string.Join(", ",
                    roleResult.Errors.Select(e => e.Description)));
            }

            return new UserDto
            {
                Id = user.Id,
                Email = user.Email!,
                DisplayName = user.DisplayName,
                EmployeeId = user.EmployeeId,
                EmployeeName = employee.FullName,
                IsActive = user.IsActive,
                Roles = dto.Roles
            };
        }
        public async Task<bool> UpdateAsync(string id, UpdateUserDto dto)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return false;
            }

            var existingUser = await _userManager.FindByEmailAsync(dto.Email);

            if (existingUser != null && existingUser.Id != id)
            {
                throw new Exception("Email is already in use.");
            }

            user.Email = dto.Email;
            user.UserName = dto.Email;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                throw new Exception(string.Join(", ",
                    result.Errors.Select(e => e.Description)));
            }

            return true;
        }
        
        public async Task<bool> UpdateRolesAsync(string id, UpdateUserRolesDto dto)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return false;
            }

            if (dto.Roles.Count <= 0)
            {
                throw new Exception("At least one role must be assigned.");
            }

            foreach (var role in dto.Roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    throw new Exception($"Role '{role}' does not exist.");
                }
            }

            var currentRoles = await _userManager.GetRolesAsync(user);

            var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);

            if (!removeResult.Succeeded)
            {
                throw new Exception(string.Join(", ",
                    removeResult.Errors.Select(e => e.Description)));
            }

            var addResult = await _userManager.AddToRolesAsync(user, dto.Roles);

            if (!addResult.Succeeded)
            {
                throw new Exception(string.Join(", ",
                    addResult.Errors.Select(e => e.Description)));
            }

            return true;
        }

        public Task<bool> ActivateAsync(string id)
        {
            return SetUserStatusAsync(id, true);
        }

        public Task<bool> DeactivateAsync(string id)
        {
            return SetUserStatusAsync(id, false);
        }


        public async Task<bool> ResetPasswordAsync(string id, ResetPasswordDto dto)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return false;
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var result = await _userManager.ResetPasswordAsync(
                user,
                token,
                dto.NewPassword);

            if (!result.Succeeded)
            {
                throw new Exception(string.Join(", ",
                    result.Errors.Select(e => e.Description)));
            }

            return true;
        }

        private async Task<UserDto> MapToUserDtoAsync(ApplicationUser user)
        {
            return new UserDto
            {
                Id = user.Id,
                Email = user.Email!,
                DisplayName = user.DisplayName,
                EmployeeId = user.EmployeeId,
                EmployeeName = user.Employee?.FullName,
                IsActive = user.IsActive,
                Roles = (await _userManager.GetRolesAsync(user)).ToList()
            };
        }

        private async Task<bool> SetUserStatusAsync(string id, bool isActive)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return false;
            }

            user.IsActive = isActive;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                throw new Exception(string.Join(", ",
                    result.Errors.Select(e => e.Description)));
            }

            return true;
        }

        public async Task<IList<string>?> GetUserRolesAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return null;

            return await _userManager.GetRolesAsync(user);
        }
    }
}
