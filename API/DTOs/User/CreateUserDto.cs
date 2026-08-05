using System.ComponentModel.DataAnnotations;

namespace API.DTOs.User
{
    public class CreateUserDto
    {
        [Required]
        public int EmployeeId { get; set; }

        [Required]
        public List<string> Roles { get; set; } = [];
    }
}   