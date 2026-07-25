using API.Models.Identity;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Employee;

public class UpdateEmployeeDto
{
    [Required]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Phone]
    public string Phone { get; set; } = string.Empty;

    [Required]
    public DateOnly JoiningDate { get; set; }

    [Required]
    public int DepartmentId { get; set; }

    [Required]
    public int DesignationId { get; set; }
    public int? ManagerId { get; set; }
    public string? ProfileImage { get; set; }
}