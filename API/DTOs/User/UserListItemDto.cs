namespace API.DTOs.User
{
    public class UserListItemDto
    {
        public string Id { get; set; } = string.Empty;
        public int EmployeeId { get; set; }
        public string EmployeeCode { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string? ProfileImage { get; set; }
        public bool IsActive { get; set; }
        public List<string> Roles { get; set; } = [];
    }
}
