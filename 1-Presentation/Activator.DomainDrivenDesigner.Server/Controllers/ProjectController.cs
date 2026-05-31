using Activator.DomainDrivenDesigner.Application.AppRequests;
using Activator.DomainDrivenDesigner.Application.AppResponses;
using Activator.DomainDrivenDesigner.Application.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Activator.DomainDrivenDesigner.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
[EnableCors("AllowDDDClientPolicy")]
public class ProjectController(ProjectAppService projectAppService) : ControllerBase
{
    private readonly ProjectAppService _projectAppService = projectAppService;

    [HttpGet("create")]
    public async Task<ActionResult<CreateProjectAppResponse>> CreateProject([FromQuery] string projectName)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (string.IsNullOrWhiteSpace(projectName))
        {
            return BadRequest("Project Name is required.");
        }

        var response = await _projectAppService.Create(
            new CreateProjectAppRequest(Guid.NewGuid(), projectName))
            .ConfigureAwait(false);

        return response.Success ? Ok() : BadRequest(response);
    }

    [HttpGet("loadall")]
    public async Task<ActionResult<RetrieveFullProjectAppResponse>> LoadFullProjects()
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var response = await _projectAppService.RetrieveFullProjects(
            new RetrieveFullProjectAppRequest(Guid.NewGuid()))
            .ConfigureAwait(false);

        return response.Success ? Ok(response) : BadRequest(response);
    }
}
