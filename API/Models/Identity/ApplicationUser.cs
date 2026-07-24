using API.Models.Entities;
using Microsoft.AspNetCore.Identity;

namespace API.Models.Identity;

public class ApplicationUser : IdentityUser
{
    public int? EmployeeId { get; set; }
    public Employee? Employee { get; set; }
}
