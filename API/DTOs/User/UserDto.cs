namespace API.DTOs.User
{
    public class UserDto
    {
        public string Id { get; set; } = "";
        public string Email { get; set; } = "";
        public string DisplayName { get; set; } = "";
        public bool IsActive { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; } = "";
        public IList<string> Roles { get; set; } = [];
    }
}
