using API.Authorization;
using API.Models.Authorization;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Seed;

public static class PermissionSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        var existingPermissions = await context.Permissions
            .ToDictionaryAsync(p => p.Name);

        foreach (var permission in PermissionDefinitions.GetAll())
        {
            if (existingPermissions.TryGetValue(permission.Name, out var existing))
            {
                existing.DisplayName = permission.DisplayName;
                existing.Category = permission.Category;
                existing.Description = permission.Description;
            }
            else
            {
                context.Permissions.Add(new Permission
                {
                    Name = permission.Name,
                    DisplayName = permission.DisplayName,
                    Category = permission.Category,
                    Description = permission.Description
                });
            }
        }

        await context.SaveChangesAsync();
    }
}