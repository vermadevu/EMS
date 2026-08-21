using API.Data;
using API.Models.Entities;
using API.Models.Enums;
using API.Models.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Net.Http.Json;
using System.Text.Json;

namespace API.Tests.Integration;

public sealed class ApiWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string _databaseName = $"ApiIntegrationTests-{Guid.NewGuid():N}";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");
        builder.ConfigureServices(services =>
        {
            services.RemoveAll<DbContextOptions<ApplicationDbContext>>();
            services.RemoveAll<IDbContextOptionsConfiguration<ApplicationDbContext>>();
            services.RemoveAll<ApplicationDbContext>();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase(_databaseName));
        });
    }

    public async Task<string> CreateAccessTokenForRoleAsync(string role)
    {
        using var scope = Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        var email = $"{role.ToLowerInvariant()}-{Guid.NewGuid():N}@dems.test";
        var employee = new Employee
        {
            EmployeeCode = $"T{Random.Shared.Next(10000, 99999)}",
            FirstName = "Test",
            LastName = role,
            Email = email,
            Phone = "9999999999",
            JoiningDate = DateOnly.FromDateTime(DateTime.Today),
            Status = EmployeeStatus.Active
        };

        context.Employees.Add(employee);
        await context.SaveChangesAsync();

        var user = new ApplicationUser
        {
            EmployeeId = employee.Id,
            DisplayName = employee.FullName,
            UserName = email,
            Email = email,
            EmailConfirmed = true,
            IsActive = true
        };

        var createResult = await userManager.CreateAsync(user, "Password@123");
        if (!createResult.Succeeded)
        {
            throw new InvalidOperationException(string.Join(", ", createResult.Errors.Select(error => error.Description)));
        }

        var roleResult = await userManager.AddToRoleAsync(user, role);
        if (!roleResult.Succeeded)
        {
            throw new InvalidOperationException(string.Join(", ", roleResult.Errors.Select(error => error.Description)));
        }

        using var client = CreateClient();
        var response = await client.PostAsJsonAsync("/api/account/login", new
        {
            Email = email,
            Password = "Password@123"
        });
        response.EnsureSuccessStatusCode();

        using var document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        return document.RootElement.GetProperty("accessToken").GetString()!;
    }
}