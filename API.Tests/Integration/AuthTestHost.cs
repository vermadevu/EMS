using API.Data;
using API.Models.Entities;
using API.Models.Enums;
using API.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace API.Tests.Integration;

public sealed class AuthTestHost : IDisposable
{
    private readonly ServiceProvider _provider;
    private readonly IServiceScope _scope;

    public AuthTestHost(bool isActive = true)
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddHttpContextAccessor();
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseInMemoryDatabase(Guid.NewGuid().ToString()));
        services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        _provider = services.BuildServiceProvider();
        _scope = _provider.CreateScope();

        UserManager = _scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        RoleManager = _scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        SignInManager = _scope.ServiceProvider.GetRequiredService<SignInManager<ApplicationUser>>();
        Context = _scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        SeedUserAsync(isActive).GetAwaiter().GetResult();
    }

    public ApplicationDbContext Context { get; }
    public UserManager<ApplicationUser> UserManager { get; }
    public RoleManager<IdentityRole> RoleManager { get; }
    public SignInManager<ApplicationUser> SignInManager { get; }
    public ApplicationUser User { get; private set; } = null!;

    private async Task SeedUserAsync(bool isActive)
    {
        foreach (var role in new[] { "Admin", "Employee" })
        {
            await RoleManager.CreateAsync(new IdentityRole(role));
        }

        var employee = new Employee
        {
            EmployeeCode = "E1000",
            FirstName = "Test",
            LastName = "User",
            Email = "user@dems.test",
            Phone = "9999999999",
            JoiningDate = DateOnly.FromDateTime(DateTime.Today),
            Status = EmployeeStatus.Active
        };

        Context.Employees.Add(employee);
        await Context.SaveChangesAsync();

        User = new ApplicationUser
        {
            Id = "test-user-id",
            EmployeeId = employee.Id,
            DisplayName = employee.FullName,
            UserName = employee.Email,
            Email = employee.Email,
            EmailConfirmed = true,
            IsActive = isActive,
            Employee = employee
        };

        var result = await UserManager.CreateAsync(User, "Password@123");
        if (!result.Succeeded)
        {
            throw new InvalidOperationException(string.Join(", ", result.Errors.Select(error => error.Description)));
        }
    }

    public void Dispose()
    {
        _scope.Dispose();
        _provider.Dispose();
    }
}