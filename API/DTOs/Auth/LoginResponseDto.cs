using API.Models.Enums;

namespace API.DTOs.Auth
{
    public class LoginResponseDto : RefreshRequestDto
    {
        public string AccessToken { get; set; } = "";
        public string Email { get; set; } = "";
        public string UserName { get; set; } = "";
        public EmployeeStatus EmployeeStatus { get; set; }
        public IList<string> Roles { get; set; } = [];
    }
}
