using API.DTOs.Employee;
using API.Exceptions;
using API.Interfaces.Repository;
using API.Interfaces.Service;
using API.Mapping;
using API.Models.Entities;
using API.Models.Enums;
using API.Services;
using AutoMapper;
using Moq;
using Xunit;

namespace API.Tests.Services;

public class EmployeeServiceTests
{
    private readonly Mock<IEmployeeRepository> _repository = new();
    private readonly Mock<ICurrentUserService> _currentUser = new();
    private readonly EmployeeService _service;

    public EmployeeServiceTests()
    {
        var mapper = new MapperConfiguration(configuration =>
            configuration.AddProfile<MappingProfile>()).CreateMapper();

        _service = new EmployeeService(_repository.Object, _currentUser.Object, mapper);
    }

    [Fact]
    public async Task CreateAsync_ShouldAssignNextCodeAndPendingStatus()
    {
        var request = CreateRequest();
        _repository.Setup(repository => repository.ExistsByEmailAsync(request.Email)).ReturnsAsync(false);
        _repository.Setup(repository => repository.DepartmentExistsAsync(request.DepartmentId)).ReturnsAsync(true);
        _repository.Setup(repository => repository.DesignationExistsAsync(request.DesignationId)).ReturnsAsync(true);
        _repository.Setup(repository => repository.GetLastEmployeeCodeAsync()).ReturnsAsync("E0007");

        Employee? addedEmployee = null;
        _repository
            .Setup(repository => repository.AddAsync(It.IsAny<Employee>()))
            .Callback<Employee>(employee => addedEmployee = employee)
            .Returns(Task.CompletedTask);

        var result = await _service.CreateAsync(request);

        Assert.NotNull(addedEmployee);
        Assert.Equal("E0008", addedEmployee!.EmployeeCode);
        Assert.Equal(EmployeeStatus.Pending, addedEmployee.Status);
        Assert.Equal(request.FirstName, result.FirstName);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowAndNotAdd_WhenEmailAlreadyExists()
    {
        var request = CreateRequest();
        _repository.Setup(repository => repository.ExistsByEmailAsync(request.Email)).ReturnsAsync(true);

        await Assert.ThrowsAsync<BadRequestException>(() => _service.CreateAsync(request));

        _repository.Verify(repository => repository.AddAsync(It.IsAny<Employee>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrow_WhenDepartmentDoesNotExist()
    {
        var request = CreateRequest();
        _repository.Setup(repository => repository.ExistsByEmailAsync(request.Email)).ReturnsAsync(false);
        _repository.Setup(repository => repository.DepartmentExistsAsync(request.DepartmentId)).ReturnsAsync(false);

        await Assert.ThrowsAsync<NotFoundException>(() => _service.CreateAsync(request));

        _repository.Verify(repository => repository.AddAsync(It.IsAny<Employee>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_ShouldRejectEmployeeAsTheirOwnManager()
    {
        var request = new UpdateEmployeeDto
        {
            FirstName = "Test",
            LastName = "Employee",
            Email = "employee@dems.test",
            Phone = "9999999999",
            JoiningDate = DateOnly.FromDateTime(DateTime.Today),
            DepartmentId = 2,
            DesignationId = 3,
            ManagerId = 4
        };
        _repository.Setup(repository => repository.DepartmentExistsAsync(request.DepartmentId)).ReturnsAsync(true);
        _repository.Setup(repository => repository.DesignationExistsAsync(request.DesignationId)).ReturnsAsync(true);

        await Assert.ThrowsAsync<BadRequestException>(() => _service.UpdateAsync(4, request));

        _repository.Verify(repository => repository.GetByIdAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task ActivateEmployeeAsync_ShouldSetActiveStatus()
    {
        var employee = CreateEmployee(4, EmployeeStatus.DocumentsSubmitted);
        _repository.Setup(repository => repository.GetByIdAsync(employee.Id)).ReturnsAsync(employee);

        var result = await _service.ActivateEmployeeAsync(employee.Id);

        Assert.True(result);
        Assert.Equal(EmployeeStatus.Active, employee.Status);
        _repository.Verify(repository => repository.UpdateAsync(employee), Times.Once);
    }

    [Fact]
    public async Task DeactivateEmployeeAsync_ShouldSetInactiveStatus()
    {
        var employee = CreateEmployee(4, EmployeeStatus.Active);
        _repository.Setup(repository => repository.GetByIdAsync(employee.Id)).ReturnsAsync(employee);

        var result = await _service.DeactivateEmployeeAsync(employee.Id);

        Assert.True(result);
        Assert.Equal(EmployeeStatus.Inactive, employee.Status);
        _repository.Verify(repository => repository.UpdateAsync(employee), Times.Once);
    }

    [Fact]
    public async Task CompleteOnboardingAsync_ShouldUseCurrentEmployeeAndSubmitDocuments()
    {
        var employee = CreateEmployee(9, EmployeeStatus.Pending);
        _currentUser.Setup(service => service.EmployeeId).Returns(employee.Id);
        _repository.Setup(repository => repository.GetByIdAsync(employee.Id)).ReturnsAsync(employee);

        var result = await _service.CompleteOnboardingAsync();

        Assert.True(result);
        Assert.Equal(EmployeeStatus.DocumentsSubmitted, employee.Status);
        _repository.Verify(repository => repository.UpdateAsync(employee), Times.Once);
    }

    private static CreateEmployeeDto CreateRequest()
    {
        return new CreateEmployeeDto
        {
            FirstName = "Test",
            LastName = "Employee",
            Email = "employee@dems.test",
            Phone = "9999999999",
            JoiningDate = DateOnly.FromDateTime(DateTime.Today),
            DepartmentId = 2,
            DesignationId = 3
        };
    }

    private static Employee CreateEmployee(int id, EmployeeStatus status)
    {
        return new Employee
        {
            Id = id,
            EmployeeCode = $"E{id:D4}",
            FirstName = "Test",
            LastName = "Employee",
            Email = "employee@dems.test",
            Status = status,
            Department = new Department { Id = 2, Name = "Engineering" },
            Designation = new Designation { Id = 3, Name = "Developer" }
        };
    }
}