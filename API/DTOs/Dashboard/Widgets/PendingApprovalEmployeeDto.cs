namespace API.DTOs.Dashboard.Widgets;

public class PendingApprovalEmployeeDto
{
    public int Id { get; set; }
    public string EmployeeCode { get; set; } = "";
    public string FullName { get; set; } = "";
    public string Department { get; set; } = "";
    public string Designation { get; set; } = "";
    public string Status { get; set; } = "";
    public string? ProfileImage { get; set; }
}