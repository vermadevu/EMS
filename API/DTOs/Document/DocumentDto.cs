using API.Models.Enums;

namespace API.DTOs.Document;

public class DocumentDto
{
    public int Id { get; set; }
    public string OriginalFileName { get; set; } = "";
    public DocumentType DocumentType { get; set; }
    public long FileSize { get; set; }
    public DateTime UploadedOn { get; set; }
    public int EmployeeId { get; set; }
    public string EmployeeName { get; set; } = "";
}