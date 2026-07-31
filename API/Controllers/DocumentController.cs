using API.Authorization;
using API.DTOs.Document;
using API.Helpers.Pagination;
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
    [HasPermission(Permissions.Documents.Read)]
    public async Task<ActionResult<IEnumerable<DocumentDto>>> GetDocuments()
    {
        var documents = await _documentService.GetAllAsync();

        return Ok(documents);
    }

    [HttpGet("{id:int}")]
    [HasPermission(Permissions.Documents.Read)]
    public async Task<ActionResult<DocumentDto>> GetDocument(int id)
    {
        var document = await _documentService.GetByIdAsync(id);

        if (document == null)
            return NotFound();

        return Ok(document);
    }

    [HttpGet("employee/{employeeId:int}")]
    [HasPermission(Permissions.Documents.Read)]
    public async Task<ActionResult<IEnumerable<DocumentDto>>> GetEmployeeDocuments(int employeeId)
    {
        return Ok(await _documentService.GetByEmployeeIdAsync(employeeId));
    }

    [HttpGet("my-documents")]
    [HasPermission(Permissions.Documents.ReadOwn)]
    public async Task<ActionResult<IEnumerable<DocumentDto>>> GetMyDocuments()
    {
        return Ok(await _documentService.GetMyDocumentsAsync());
    }


    [HttpPost]
    [HasPermission(Permissions.Documents.Upload)]
    public async Task<ActionResult<DocumentDto>> UploadDocument([FromForm] UploadDocumentDto dto)
    {
        var document = await _documentService.UploadAsync(dto);

        return CreatedAtAction(
            nameof(GetDocument),
            new { id = document.Id },
            document);
    }

    [HttpDelete("{id:int}")]
    [HasPermission(Permissions.Documents.Delete)]
    public async Task<IActionResult> DeleteDocument(int id)
    {
        var deleted = await _documentService.DeleteAsync(id);

        if (!deleted)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("my-documents/{id:int}")]
    [HasPermission(Permissions.Documents.DeleteOwn)]
    public async Task<IActionResult> DeleteMyDocument(int id)
    {
        var deleted = await _documentService.DeleteMyDocumentAsync(id);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpGet("employee")]
    [HasPermission(Permissions.Documents.Read)]
    public async Task<ActionResult<PagedResult<EmployeeDocumentSummaryDto>>> GetEmployeeDocumentSummary([FromQuery] DocumentQueryParams queryParams)
    {
        return Ok(
            await _documentService
                .GetEmployeeDocumentSummaryAsync(queryParams));
    }

}