using Activator.DomainDrivenDesigner.Application.AppRequests;
using Activator.DomainDrivenDesigner.Application.AppResponses;
using Activator.DomainDrivenDesigner.Application.Services;
using Common.Core.AOP.LogTrace;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Activator.DomainDrivenDesigner.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
[EnableCors("AllowDDDClientPolicy")]
public class ProjectController(ProjectAppService projectAppService, IRequestContext requestContext) : ControllerBase
{
    private readonly ProjectAppService _projectAppService = projectAppService;
    private readonly IRequestContext _requestContext = requestContext;

    [HttpPost("create")]
    public async Task<ActionResult<CreateProjectAppResponse>> CreateProject([FromBody] CreateProjectAppRequestModel request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (string.IsNullOrWhiteSpace(request.name))
        {
            return BadRequest("Project Name is required.");
        }
        
        var requestId = Guid.Parse(_requestContext.TraceId);

        var response = await _projectAppService.Create(
            new CreateProjectAppRequest(requestId, request.name, request.description))
            .ConfigureAwait(false);

        return response.Success ? Ok() : BadRequest(response.ErrorMessage);
    }

    [HttpGet("loadall")]
    public async Task<ActionResult<RetrieveFullProjectAppResponse>> LoadFullProjects()
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var requestId = Guid.Parse(_requestContext.TraceId);

        var response = await _projectAppService.RetrieveFullProjects(
            new RetrieveFullProjectAppRequest(requestId))
            .ConfigureAwait(false);

        return response.Success ? Ok(response) : BadRequest(response);
    }
}
