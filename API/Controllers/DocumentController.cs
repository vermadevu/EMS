using API.DTOs.Document;
using API.Interfaces.Service;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class DocumentController(IDocumentService documentService) : BaseApiController
{
    private readonly IDocumentService _documentService = documentService;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DocumentDto>>> GetDocuments()
    {
        var documents = await _documentService.GetAllAsync();

        return Ok(documents);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<DocumentDto>> GetDocument(int id)
    {
        var document = await _documentService.GetByIdAsync(id);

        if (document == null)
            return NotFound();

        return Ok(document);
    }

    [HttpGet("employee/{employeeId:int}")]
    [Authorize(Roles = "Admin,HR")]
    public async Task<ActionResult<IEnumerable<DocumentDto>>> GetEmployeeDocuments(int employeeId)
    {
        return Ok(await _documentService.GetByEmployeeIdAsync(employeeId));
    }

    [HttpGet("my-documents")]
    public async Task<ActionResult<IEnumerable<DocumentDto>>> GetMyDocuments()
    {
        return Ok(await _documentService.GetMyDocumentsAsync());
    }


    [Authorize(Roles = "Admin,HR")]
    [HttpPost]
    public async Task<ActionResult<DocumentDto>> UploadDocument([FromForm] UploadDocumentDto dto)
    {
        var document = await _documentService.UploadAsync(dto);

        return CreatedAtAction(
            nameof(GetDocument),
            new { id = document.Id },
            document);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteDocument(int id)
    {
        var deleted = await _documentService.DeleteAsync(id);

        if (!deleted)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("my-documents/{id:int}")]
    public async Task<IActionResult> DeleteMyDocument(int id)
    {
        var deleted = await _documentService.DeleteMyDocumentAsync(id);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }

}