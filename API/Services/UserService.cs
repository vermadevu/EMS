using API.Constants;
using API.DTOs.User;
using API.Exceptions;
using API.Helpers.Pagination;
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
        IEmployeeRepository employeeRepository,
        IApplicationUserRepository userRepository) : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;
        private readonly IEmployeeRepository _employeeRepository = employeeRepository;
        private readonly IApplicationUserRepository _userRepository = userRepository;
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
            var user = await GetUserAsync(id);

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

        public async Task<CreateUserResponseDto> CreateAsync(CreateUserDto dto)
        {
            var employee = await _employeeRepository.GetByIdAsync(dto.EmployeeId) ?? throw new NotFoundException("Employee not found.");

            if (await _employeeRepository.HasUserAccountAsync(dto.EmployeeId))
            {
                throw new BadRequestException("User account already exists for this employee.");
            }

            var existingUser = await _userManager.FindByEmailAsync(employee.Email);

            if (existingUser != null)
            {
                throw new BadRequestException("Email is already in use.");
            }

            await ValidateRolesAsync(dto.Roles);

            var password = GenerateTemporaryPassword(employee.FirstName);

            var user = new ApplicationUser
            {
                EmployeeId = employee.Id,
                DisplayName = employee.FullName,
                UserName = employee.Email,
                Email = employee.Email,
                EmailConfirmed = true,
                IsActive = true
            };

            var result = await _userManager.CreateAsync(user, password);

            EnsureSuccess(result);

            var roleResult = await _userManager.AddToRolesAsync(user, dto.Roles);

            if (!roleResult.Succeeded)
            {
                await _userManager.DeleteAsync(user);

                EnsureSuccess(roleResult);
            }

            var roles = dto.Roles.Any()
                  ? dto.Roles
                  : new List<string> { Roles.Employee };

            await _userManager.AddToRolesAsync(user, roles);

            return new CreateUserResponseDto
            {
                Username = user.UserName!,
                TemporaryPassword = password
            };
        }
        public async Task<bool> UpdateAsync(string id, UpdateUserDto dto)
        {
            var user = await GetUserAsync(id);

            if (user == null)
            {
                return false;
            }

            var existingUser = await _userManager.FindByEmailAsync(dto.Email);

            if (existingUser != null && existingUser.Id != id)
            {
                throw new BadRequestException("Email is already in use.");
            }

            user.Email = dto.Email;
            user.UserName = dto.Email;

            var result = await _userManager.UpdateAsync(user);

            EnsureSuccess(result);

            return true;
        }

        public async Task<bool> UpdateRolesAsync(string id, UpdateUserRolesDto dto)
        {
            var user = await GetUserAsync(id);

            if (user == null)
            {
                return false;
            }

            await ValidateRolesAsync(dto.Roles);

            var currentRoles = await _userManager.GetRolesAsync(user);

            var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
            EnsureSuccess(removeResult);

            var addResult = await _userManager.AddToRolesAsync(user, dto.Roles);
            EnsureSuccess(addResult);

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
            var user = await GetUserAsync(id);

            if (user == null)
            {
                return false;
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var result = await _userManager.ResetPasswordAsync(
                user,
                token,
                dto.NewPassword);

            EnsureSuccess(result);

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
                EmployeeName = user.Employee.FullName,
                IsActive = user.IsActive,
                Roles = (await _userManager.GetRolesAsync(user)).ToList()
            };
        }

        private async Task<bool> SetUserStatusAsync(string id, bool isActive)
        {
            var user = await GetUserAsync(id);

            if (user == null)
            {
                return false;
            }

            user.IsActive = isActive;

            var result = await _userManager.UpdateAsync(user);

            EnsureSuccess(result);

            return true;
        }

        public async Task<IList<string>?> GetUserRolesAsync(string id)
        {
            var user = await GetUserAsync(id);

            if (user == null)
                return null;

            return await _userManager.GetRolesAsync(user);
        }

        private async Task ValidateRolesAsync(IList<string> roles)
        {
            if (roles == null || roles.Count == 0)
                throw new BadRequestException("At least one role must be assigned.");

            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                    throw new NotFoundException($"Role '{role}' does not exist.");
            }
        }

        private static void EnsureSuccess(IdentityResult result)
        {
            if (!result.Succeeded)
            {
                throw new BadRequestException(string.Join(", ",
                    result.Errors.Select(e => e.Description)));
            }
        }

        private Task<ApplicationUser?> GetUserAsync(string id)
        {
            return _userManager.Users
                .Include(u => u.Employee)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<List<AvailableEmployeeDto>> GetAvailableEmployeesAsync()
        {
            return await _employeeRepository
                .GetEmployeesWithoutAccountAsync();
        }

        private static string GenerateTemporaryPassword(string firstName)
        {
            firstName = firstName.Trim();

            if (!string.IsNullOrWhiteSpace(firstName))
            {
                firstName = char.ToUpper(firstName[0]) +
                            firstName.Substring(1).ToLower();
            }

            var random = Random.Shared.Next(1000, 9999);

            return $"{firstName}@DEMS{random}";
        }
        public async Task<PagedResult<UserListItemDto>> GetPagedAsync(UserQueryParams query)
        {
            return await _userRepository.GetPagedAsync(query);
        }
    }
}
