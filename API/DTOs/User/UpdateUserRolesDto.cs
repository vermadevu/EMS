using System.ComponentModel.DataAnnotations;

namespace API.DTOs.User
{
    public class UpdateUserRolesDto
    {
        [Required]
        public List<string> Roles { get; set; } = [];
    }
}
