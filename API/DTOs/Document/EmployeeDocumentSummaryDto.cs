namespace API.DTOs.Document
{
public class EmployeeDocumentSummaryDto
{
    public int EmployeeId { get; set; }
    public string EmployeeCode { get; set; } = "";
    public string FullName { get; set; } = "";
    public string? ProfileImage { get; set; }
    public string Department { get; set; } = "";
    public int TotalDocuments { get; set; }
}
}
