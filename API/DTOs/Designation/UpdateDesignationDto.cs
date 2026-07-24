using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Designation;

public class UpdateDesignationDto
{
    [Required]
    public string Name { get; set; } = "";

    public string? Description { get; set; }
}