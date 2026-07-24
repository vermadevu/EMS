using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Designation;

public class CreateDesignationDto
{
    [Required]
    public string Name { get; set; } = "";

    public string? Description { get; set; }
}