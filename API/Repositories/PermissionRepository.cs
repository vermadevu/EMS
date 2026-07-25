using API.Data;
using API.Interfaces.Repository;
using API.Models.Authorization;
using API.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories;

public class PermissionRepository(
    ApplicationDbContext context,
    UserManager<ApplicationUser> userManager)
    : IPermissionRepository
{
    private readonly ApplicationDbContext _context = context;
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    public async Task<HashSet<string>> GetEffectivePermissionsAsync(string userId)
    {
        var permissions = new HashSet<string>();

        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            return permissions;

        // Step 1 - Role Permissions

        var roles = await _userManager.GetRolesAsync(user);

        var roleIds = await _context.Roles
            .Where(r => roles.Contains(r.Name!))
            .Select(r => r.Id)
            .ToListAsync();

        var rolePermissions = await _context.RolePermissions
            .Where(rp => roleIds.Contains(rp.RoleId))
            .Select(rp => rp.Permission.Name)
            .ToListAsync();

        permissions.UnionWith(rolePermissions);

        // Step 2 - User Overrides

        var overrides = await _context.UserPermissions
            .Include(up => up.Permission)
            .Where(up => up.UserId == userId)
            .ToListAsync();

        foreach (var permission in overrides)
        {
            if (permission.IsAllowed)
            {
                permissions.Add(permission.Permission.Name);
            }
            else
            {
                permissions.Remove(permission.Permission.Name);
            }
        }

        return permissions;
    }

    public async Task<IEnumerable<Permission>> GetAllAsync()
    {
        return await _context.Permissions
            .OrderBy(p => p.Category)
            .ThenBy(p => p.DisplayName)
            .ToListAsync();
    }

}