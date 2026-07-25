using API.Data.Seed;
using API.Models.Entities;
using API.Models.Enums;
using API.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public static class DbInitializer
{
    public static async Task SeedAsync(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {

        //if (await context.Permissions.AnyAsync())
        //    return;

        // Seed Roles
        string[] roles =
        {
            "Admin",
            "HR",
            "Manager",
            "Employee"
        };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        await PermissionSeeder.SeedAsync(context);

        await RolePermissionSeeder.SeedAsync(context, roleManager);

        const string adminEmail = "admin@dems.com";

        // Seed Department
        var department = await context.Departments
            .FirstOrDefaultAsync(d => d.Name == "IT");

        if (department == null)
        {
            department = new Department
            {
                Name = "IT",
                Description = "Information Technology"
            };

            context.Departments.Add(department);
            await context.SaveChangesAsync();
        }

        // Seed Designation
        var designation = await context.Designations
            .FirstOrDefaultAsync(d => d.Name == "Administrator");

        if (designation == null)
        {
            designation = new Designation
            {
                Name = "Administrator",
                Description = "System Administrator"
            };

            context.Designations.Add(designation);
            await context.SaveChangesAsync();
        }

        // Seed Employee
        var employee = await context.Employees
            .FirstOrDefaultAsync(e => e.Email == adminEmail);

        if (employee == null)
        {
            employee = new Employee
            {
                EmployeeCode = "E0001",
                FirstName = "System",
                LastName = "Administrator",
                Email = adminEmail,
                Phone = "9999999999",
                JoiningDate = DateOnly.FromDateTime(DateTime.Today),
                DepartmentId = department.Id,
                DesignationId = designation.Id,
                Status = EmployeeStatus.Active
            };

            context.Employees.Add(employee);
            await context.SaveChangesAsync();
        }

        // Seed Admin User
        var adminUser = await userManager.Users
            .FirstOrDefaultAsync(u => u.Email == adminEmail);

        if (adminUser == null)
        {
            adminUser = new ApplicationUser
            {
                EmployeeId = employee.Id,
                DisplayName = employee.FullName,
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true,
                IsActive = true
            };

            var result = await userManager.CreateAsync(adminUser, "Admin@123");

            if (!result.Succeeded)
            {
                throw new Exception(string.Join(", ",
                    result.Errors.Select(e => e.Description)));
            }

            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }
}