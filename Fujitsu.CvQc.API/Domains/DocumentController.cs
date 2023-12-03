using Fujitsu.CvQc.Business;
using Microsoft.AspNetCore.Mvc;

namespace Fujitsu.CvQc.API.Domains;

[Route("api/[controller]")]
[ApiController]
public class DocumentController : ControllerBase
{
    private readonly IDocumentService documentService = ServiceInjector.serviceProvider!.GetService<IDocumentService>()!;

    [HttpGet("/api/project/{projectId}/[controller]/list")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<DocumentModel>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<DocumentModel>>> GetDocuments(string projectId)
    {
        var task = Task.Run(() => documentService.GetDocuments(projectId));
        return await task;
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DocumentModel))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DocumentModel?>> GetDocument(string id)
    {
        var task = Task.Run(() => documentService.GetDocument(id));
        return await task;
    }
}
