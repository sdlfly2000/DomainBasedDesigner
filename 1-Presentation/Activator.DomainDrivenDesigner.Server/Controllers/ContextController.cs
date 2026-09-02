using Activator.DomainDrivenDesigner.Application.AppRequests;
using Activator.DomainDrivenDesigner.Application.Services;
using Activator.DomainDrivenDesigner.Server.Models;
using Common.Core.AOP.LogTrace;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Activator.DomainDrivenDesigner.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
[EnableCors("AllowDDDClientPolicy")]
public class ContextController : ControllerBase
{
    private readonly ContextAppService _contextAppService;
    private readonly IRequestContext _requestContext;

    public ContextController(ContextAppService contextAppService, IRequestContext requestContext)
    {
        _contextAppService = contextAppService;
        _requestContext = requestContext;
    }

    [HttpGet("retrieve/{projectId}")]
    public async Task<IActionResult> Retrieve(string projectId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var requestId = Guid.Parse(_requestContext.TraceId);

        var response = await _contextAppService.RetrieveContexts(new RetrieveContextAppRequest(requestId, Guid.Parse(projectId))).ConfigureAwait(false);
        
        return response.Success ? Ok(response.Contexts) : Problem(response.ErrorMessage, statusCode: 500);
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] CreateContextRequestModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var requestId = Guid.Parse(_requestContext.TraceId);

        var response = await _contextAppService.CreateContext(new CreateContextAppRequest(requestId, model.name, model.projectId)).ConfigureAwait(false);

        return response.Success ? Ok(response.ContextId) : Problem(response.ErrorMessage, statusCode: 500);
    }
}