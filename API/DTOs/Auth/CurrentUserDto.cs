using API.Models.Enums;

namespace API.DTOs.Auth;

public class CurrentUserDto
{
    public string Id { get; set; } = string.Empty;
    public int EmployeeId { get; set; }
    public string DisplayName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public EmployeeStatus EmployeeStatus { get; set; }
    public IList<string> Roles { get; set; } = [];
    public HashSet<string> Permissions { get; set; } = [];
    public string? ProfileImage { get; set; }
}