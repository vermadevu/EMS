using System.ComponentModel.DataAnnotations;

namespace API.DTOs.User
{
    public class UpdateUserDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";
    }
}
