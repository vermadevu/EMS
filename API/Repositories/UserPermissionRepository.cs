using API.Data;
using API.DTOs.Permission;
using API.DTOs.UserPermissionManagement;
using API.Exceptions;
using API.Interfaces.Repository;
using API.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class UserPermissionRepository(ApplicationDbContext context, UserManager<ApplicationUser> userManager) : IUserPermissionRepository
    {
        private readonly ApplicationDbContext _context = context;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        public async Task<IEnumerable<UserListDto>> GetUsersAsync()
        {
            var users = await _userManager.Users
                .OrderBy(u => u.DisplayName)
                .ToListAsync();

            var result = new List<UserListDto>();

            foreach (var user in users)
            {
                var role = (await _userManager.GetRolesAsync(user))
                    .FirstOrDefault() ?? string.Empty;

                result.Add(new UserListDto
                {
                    UserId = user.Id,
                    DisplayName = user.DisplayName,
                    Email = user.Email!,
                    Role = role
                });
            }

            return result;
        }
        public async Task<bool> AreValidPermissionIdsAsync(List<int> permissionIds)
        {
            var count = await _context.Permissions
                .CountAsync(p => permissionIds.Contains(p.Id));

            return count == permissionIds.Count;
        }

        public async Task<UserPermissionsDto> GetUserPermissionsAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId) ?? throw new NotFoundException("User not found.");

            var roleName = (await _userManager.GetRolesAsync(user))
                .FirstOrDefault();

            if (string.IsNullOrWhiteSpace(roleName))
            {
                throw new NotFoundException("User role not found.");
            }

            var roleId = await _context.Roles
                .Where(r => r.Name == roleName)
                .Select(r => r.Id)
                .FirstAsync();

            var permissions = await _context.Permissions
                .OrderBy(p => p.Category)
                .ThenBy(p => p.DisplayName)
                .ToListAsync();

            var rolePermissionIds = await _context.RolePermissions
                .Where(rp => rp.RoleId == roleId)
                .Select(rp => rp.PermissionId)
                .ToHashSetAsync();

            var overrides = await _context.UserPermissions
                .Where(up => up.UserId == userId)
                .ToDictionaryAsync(
                    up => up.PermissionId,
                    up => up.IsAllowed);

            return new UserPermissionsDto
            {
                UserId = user.Id,
                DisplayName = user.DisplayName,
                Email = user.Email!,
                Role = roleName,
                Categories = permissions
                    .GroupBy(p => p.Category)
                    .OrderBy(g => g.Key)
                    .Select(group =>
                    {
                        var permissionDtos = group
                            .OrderBy(p => p.DisplayName)
                            .Select(permission =>
                            {
                                bool assigned;

                                if (overrides.TryGetValue(permission.Id, out var isAllowed))
                                {
                                    assigned = isAllowed;
                                }
                                else
                                {
                                    assigned = rolePermissionIds.Contains(permission.Id);
                                }

                                return new PermissionAssignmentDto
                                {
                                    PermissionId = permission.Id,
                                    Name = permission.Name,
                                    DisplayName = permission.DisplayName,
                                    Description = permission.Description,
                                    Assigned = assigned
                                };
                            })
                            .ToList();

                        return new PermissionCategoryDto
                        {
                            Name = group.Key,
                            TotalPermissions = permissionDtos.Count,
                            AssignedPermissions = permissionDtos.Count(p => p.Assigned),
                            Permissions = permissionDtos
                        };
                    })
                    .ToList()
            };


        }

        public async Task UpdateUserPermissionsAsync(string userId, List<UserPermissionOverrideDto> permissions)
        {
            var user = await _userManager.FindByIdAsync(userId) ?? throw new NotFoundException("User not found.");

            await using var transaction =
                await _context.Database.BeginTransactionAsync();

            try
            {
                var existingOverrides = await _context.UserPermissions
                    .Where(up => up.UserId == userId)
                    .ToListAsync();

                _context.UserPermissions.RemoveRange(existingOverrides);

                if (permissions.Count > 0)
                {
                    var overrides = permissions.Select(permission =>
                        new Models.Authorization.UserPermission
                        {
                            UserId = userId,
                            PermissionId = permission.PermissionId,
                            IsAllowed = permission.IsAllowed
                        });

                    await _context.UserPermissions.AddRangeAsync(overrides);
                }

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
