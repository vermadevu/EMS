using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Asset;

public class AssignAssetDto
{
    [Required]
    public int EmployeeId { get; set; }
}