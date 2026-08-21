using API.DTOs.User;
using API.Exceptions;
using API.Interfaces.Repository;
using API.Models.Entities;
using API.Models.Identity;
using API.Services;
using Moq;
using Xunit;

namespace API.Tests.Services;

public class UserServiceTests
{
    [Fact]
    public async Task CreateAsync_ShouldCreateUserWithRequestedRole()
    {
        using var host = new API.Tests.Integration.AuthTestHost();
        var employee = CreateEmployee(20, "new.user@dems.test");
        var employeeRepository = new Mock<IEmployeeRepository>();
        employeeRepository.Setup(repository => repository.GetByIdAsync(employee.Id)).ReturnsAsync(employee);
        employeeRepository.Setup(repository => repository.HasUserAccountAsync(employee.Id)).ReturnsAsync(false);
        var service = CreateService(host, employeeRepository);

        var result = await service.CreateAsync(new CreateUserDto
        {
            EmployeeId = employee.Id,
            Roles = ["Employee"]
        });

        Assert.Equal(employee.Email, result.Username);
        Assert.Matches($"^{employee.FirstName}@DEMS\\d{{4}}$", result.TemporaryPassword);

        var createdUser = await host.UserManager.FindByEmailAsync(employee.Email);
        Assert.NotNull(createdUser);
        Assert.Contains("Employee", await host.UserManager.GetRolesAsync(createdUser!));
    }

    [Fact]
    public async Task CreateAsync_ShouldThrow_WhenEmployeeDoesNotExist()
    {
        using var host = new API.Tests.Integration.AuthTestHost();
        var employeeRepository = new Mock<IEmployeeRepository>();
        employeeRepository.Setup(repository => repository.GetByIdAsync(404)).ReturnsAsync((Employee?)null);
        var service = CreateService(host, employeeRepository);

        await Assert.ThrowsAsync<NotFoundException>(() => service.CreateAsync(new CreateUserDto
        {
            EmployeeId = 404,
            Roles = ["Employee"]
        }));
    }

    [Fact]
    public async Task CreateAsync_ShouldThrow_WhenNoRolesAreProvided()
    {
        using var host = new API.Tests.Integration.AuthTestHost();
        var employee = CreateEmployee(21, "roleless@dems.test");
        var employeeRepository = new Mock<IEmployeeRepository>();
        employeeRepository.Setup(repository => repository.GetByIdAsync(employee.Id)).ReturnsAsync(employee);
        employeeRepository.Setup(repository => repository.HasUserAccountAsync(employee.Id)).ReturnsAsync(false);
        var service = CreateService(host, employeeRepository);

        await Assert.ThrowsAsync<BadRequestException>(() => service.CreateAsync(new CreateUserDto
        {
            EmployeeId = employee.Id,
            Roles = []
        }));
    }

    [Fact]
    public async Task CreateAsync_ShouldThrow_WhenEmployeeAlreadyHasAccount()
    {
        using var host = new API.Tests.Integration.AuthTestHost();
        var employee = CreateEmployee(22, "existing@dems.test");
        var employeeRepository = new Mock<IEmployeeRepository>();
        employeeRepository.Setup(repository => repository.GetByIdAsync(employee.Id)).ReturnsAsync(employee);
        employeeRepository.Setup(repository => repository.HasUserAccountAsync(employee.Id)).ReturnsAsync(true);
        var service = CreateService(host, employeeRepository);

        await Assert.ThrowsAsync<BadRequestException>(() => service.CreateAsync(new CreateUserDto
        {
            EmployeeId = employee.Id,
            Roles = ["Employee"]
        }));
    }

    [Fact]
    public async Task DeactivateAsync_ShouldSetUserInactive()
    {
        using var host = new API.Tests.Integration.AuthTestHost();
        var service = CreateService(host, new Mock<IEmployeeRepository>());

        var result = await service.DeactivateAsync(host.User.Id);

        Assert.True(result);
        var user = await host.UserManager.FindByIdAsync(host.User.Id);
        Assert.False(user!.IsActive);
    }

    [Fact]
    public async Task ResetPasswordAsync_ShouldAllowTheNewPassword()
    {
        using var host = new API.Tests.Integration.AuthTestHost();
        var service = CreateService(host, new Mock<IEmployeeRepository>());

        var result = await service.ResetPasswordAsync(host.User.Id, new ResetPasswordDto
        {
            NewPassword = "NewPassword@123"
        });

        Assert.True(result);
        Assert.True(await host.UserManager.CheckPasswordAsync(host.User, "NewPassword@123"));
    }

    private static UserService CreateService(
        API.Tests.Integration.AuthTestHost host,
        Mock<IEmployeeRepository> employeeRepository)
    {
        return new UserService(
            host.UserManager,
            host.RoleManager,
            employeeRepository.Object,
            new Mock<IApplicationUserRepository>().Object);
    }

    private static Employee CreateEmployee(int id, string email)
    {
        return new Employee
        {
            Id = id,
            EmployeeCode = $"E{id:D4}",
            FirstName = "New",
            LastName = "User",
            Email = email,
            Phone = "9999999999"
        };
    }
}