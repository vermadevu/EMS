using API.DTOs.Document;
using API.Interfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class DocumentController(IDocumentService service) : BaseApiController
{
    private readonly IDocumentService _service = service;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DocumentDto>>> GetDocuments()
    {
        var documents = await _service.GetAllAsync();

        return Ok(documents);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<DocumentDto>> GetDocument(int id)
    {
        var document = await _service.GetByIdAsync(id);

        if (document == null)
            return NotFound();

        return Ok(document);
    }

    [HttpGet("employee/{employeeId:int}")]
    public async Task<ActionResult<IEnumerable<DocumentDto>>> GetEmployeeDocuments(int employeeId)
    {
        var documents = await _service.GetByEmployeeIdAsync(employeeId);

        return Ok(documents);
    }

    [Authorize(Roles = "Admin,HR")]
    [HttpPost]
    public async Task<ActionResult<DocumentDto>> UploadDocument([FromForm] UploadDocumentDto dto)
    {
        var document = await _service.UploadAsync(dto);

        return CreatedAtAction(
            nameof(GetDocument),
            new { id = document.Id },
            document);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteDocument(int id)
    {
        var deleted = await _service.DeleteAsync(id);

        if (!deleted)
            return NotFound();

        return NoContent();
    }
}