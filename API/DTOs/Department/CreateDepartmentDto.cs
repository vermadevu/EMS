using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Department;

public class CreateDepartmentDto
{
    [Required]
    public string Name { get; set; } = "";

    public string? Description { get; set; }
}