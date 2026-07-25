using System.ComponentModel.DataAnnotations;

namespace API.DTOs.User
{
    public class ResetPasswordDto
    {
        [Required]
        public string NewPassword { get; set; } = "";
    }
}
