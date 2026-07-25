using API.Data;
using API.DTOs.Permission;
using API.DTOs.RolePermissionManagement;
using API.Exceptions;
using API.Interfaces.Repository;
using API.Models.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class RolePermissionRepository(ApplicationDbContext context, RoleManager<IdentityRole> roleManager) : IRolePermissionRepository
    {
        private readonly ApplicationDbContext _context = context;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;

        public async Task<IEnumerable<IdentityRole>> GetRolesAsync()
        {
            return await _roleManager.Roles
                .OrderBy(r => r.Name)
                .ToListAsync();
        }

        public async Task<RolePermissionsDto> GetRolePermissionsAsync(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);

            if (role == null)
                throw new NotFoundException("Role not found.");

            var permissions = await _context.Permissions
                .OrderBy(p => p.Category)
                .ThenBy(p => p.DisplayName)
                .ToListAsync();

            var assignedPermissionIds = await _context.RolePermissions
                .Where(rp => rp.RoleId == roleId)
                .Select(rp => rp.PermissionId)
                .ToHashSetAsync();
            
            return new RolePermissionsDto
            {
                RoleId = role.Id,
                RoleName = role.Name!,
                Categories = permissions
                    .GroupBy(p => p.Category)
                    .OrderBy(g => g.Key)
                    .Select(group =>
                    {
                        var permissionDtos = group
                            .OrderBy(p => p.DisplayName)
                            .Select(permission => new PermissionAssignmentDto
                            {
                                PermissionId = permission.Id,
                                Name = permission.Name,
                                DisplayName = permission.DisplayName,
                                Description = permission.Description,
                                Assigned = assignedPermissionIds.Contains(permission.Id)
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

        public async Task UpdateRolePermissionsAsync(string roleId, List<int> permissionIds)
        {
            var role = await _roleManager.FindByIdAsync(roleId) ?? throw new NotFoundException("Role not found.");

            await using var transaction =
                await _context.Database.BeginTransactionAsync();

            try
            {
                var existingPermissions = await _context.RolePermissions
                    .Where(rp => rp.RoleId == roleId)
                    .ToListAsync();

                _context.RolePermissions.RemoveRange(existingPermissions);

                if (permissionIds.Count != 0)
                {
                    var newPermissions = permissionIds
                        .Select(permissionId => new RolePermission
                        {
                            RoleId = roleId,
                            PermissionId = permissionId
                        });

                    await _context.RolePermissions.AddRangeAsync(newPermissions);
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

        public async Task<bool> AreValidPermissionIdsAsync(List<int> permissionIds)
        {
            var count = await _context.Permissions
                .CountAsync(p => permissionIds.Contains(p.Id));

            return count == permissionIds.Count;
        }

    }
}
