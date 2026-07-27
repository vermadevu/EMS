namespace API.DTOs.Dashboard.Widgets;

public class RecentEmployeeDto
{
    public int Id { get; set; }
    public string EmployeeCode { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string Designation { get; set; } = string.Empty;
    public DateOnly JoiningDate { get; set; }
    public string Status { get; set; } = string.Empty;
}