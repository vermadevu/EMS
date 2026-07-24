using API.Models.Base;
using API.Models.Entities;
using API.Models.Enums;

public class Document : BaseEntity
{
    public string OriginalFileName { get; set; } = "";
    public string PublicId { get; set; } = "";
    public string Url { get; set; } = "";
    public string ContentType { get; set; } = "";
    public long FileSize { get; set; }
    public DocumentType DocumentType { get; set; }
    public DateTime UploadedOn { get; set; } = DateTime.UtcNow;
    public int EmployeeId { get; set; }
    public Employee Employee { get; set; } = null!;
}