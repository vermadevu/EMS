using API.Models.Enums;

namespace API.DTOs.Employee
{
    public class EmployeeListItemDto
    {
        public int Id { get; set; }
        public string EmployeeCode { get; set; } = "";
        public string FullName { get; set; } = "";
        public string Department { get; set; } = "";
        public string Designation { get; set; } = "";
        public EmployeeStatus Status { get; set; }
        public DateOnly JoiningDate { get; set; }
    }
}
