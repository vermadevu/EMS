using System.ComponentModel.DataAnnotations;

namespace API.DTOs.User
{
    public class CreateUserDto
    {
        [Required]
        public int EmployeeId { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";

        [Required]
        public string DisplayName { get; set; } = "";

        [Required]
        public string Password { get; set; } = "";

        [Required]
        public List<string> Roles { get; set; } = [];
    }
}