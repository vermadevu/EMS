namespace API.DTOs.Department
{
    public class DepartmentListItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string? Description { get; set; }
        public int EmployeeCount { get; set; }
    }
}
