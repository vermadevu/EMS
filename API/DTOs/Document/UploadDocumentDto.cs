using API.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Document;

public class UploadDocumentDto
{
    [Required]
    public DocumentType DocumentType { get; set; }

    [Required]
    public IFormFile File { get; set; } = null!;
}