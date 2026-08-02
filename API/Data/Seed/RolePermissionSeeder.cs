using API.Authorization;
using API.Constants;
using API.Models.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Seed;

public static class RolePermissionSeeder
{
    public static async Task SeedAsync(
        ApplicationDbContext context,
        RoleManager<IdentityRole> roleManager)
    {
        await SeedRoleAsync(context, roleManager, Roles.Admin, RolePermissions.Admin);
        await SeedRoleAsync(context, roleManager, Roles.HR, RolePermissions.HR);
        await SeedRoleAsync(context, roleManager, Roles.Manager, RolePermissions.Manager);
        await SeedRoleAsync(context, roleManager, Roles.Employee, RolePermissions.Employee);
    }

    private static async Task SeedRoleAsync(ApplicationDbContext context, RoleManager<IdentityRole> roleManager, string roleName, IEnumerable<string> permissions)
    {
        var role = await roleManager.FindByNameAsync(roleName);

        if (role == null)
            return;

        var existingPermissions = await context.RolePermissions
            .Where(x => x.RoleId == role.Id)
            .Select(x => x.Permission.Name)
            .ToListAsync();

        var permissionsToAdd = permissions.Except(existingPermissions);

        if (!permissionsToAdd.Any())
            return;

        var permissionEntities = await context.Permissions
            .Where(p => permissionsToAdd.Contains(p.Name))
            .ToListAsync();

        foreach (var permission in permissionEntities)
        {
            context.RolePermissions.Add(new RolePermission
            {
                RoleId = role.Id,
                PermissionId = permission.Id
            });
        }

        await context.SaveChangesAsync();
    }
}