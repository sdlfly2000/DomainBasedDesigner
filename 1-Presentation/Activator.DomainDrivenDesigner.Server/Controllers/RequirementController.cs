using Activator.DomainDrivenDesigner.Application.AppRequests;
using Activator.DomainDrivenDesigner.Application.AppResponses;
using Activator.DomainDrivenDesigner.Application.Services;
using Activator.DomainDrivenDesigner.Server.Models;
using Common.Core.AOP.LogTrace;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Activator.DomainDrivenDesigner.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
[EnableCors("AllowDDDClientPolicy")]
public class RequirementController(RequirementAppService requirementAppService, IRequestContext requestContext) : ControllerBase
{
    private readonly IRequestContext _requestContext = requestContext;

    private readonly RequirementAppService _requirementAppService = requirementAppService;

    [HttpGet("project/{projectId}")]
    public async Task<ActionResult<RetrieveRequirementByProjectAppResponse>> LoadRequirementsByProject(Guid projectId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var requestId = Guid.Parse(_requestContext.TraceId);

        var response = await _requirementAppService.RetrieveFullRequirements(
            new RetrieveRequirementByProjectAppRequest(requestId, projectId))
            .ConfigureAwait(false);

        return response.Success ? Ok(response) : BadRequest(response);
    }

    [HttpGet("retrieve/{requirementId}")]
    public async Task<ActionResult> RetrieveRequirement(Guid requirementId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var requestId = Guid.Parse(_requestContext.TraceId);

        var response = await _requirementAppService.RetrieveRequirement(requestId, requirementId).ConfigureAwait(false);

        if (response.Requirement == null)
        {
            return NotFound();
        }

        var model = new RetrieveRequirementResponseModel(response.Requirement.Id.ToString(), response.Requirement.Description);

        return response.Success ? Ok(model) : BadRequest(response);
    }

    [HttpPost("analyze")]
    public async Task<ActionResult<AnalyzeRequirementsResponse>> AnalyzeRequirements([FromBody] AnalyzeRequirementsRequestsModel request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var requestId = Guid.Parse(_requestContext.TraceId);

        var response = await _requirementAppService.AnalyzeRequirement(
            new AnalyzeRequirementsRequest(requestId, request.Description))
            .ConfigureAwait(false);

        return response.Success ? Ok(response) : BadRequest(response);
    }

    [HttpPost("save")]
    public async Task<ActionResult<SaveRequirementResponse>> SaveRequirement([FromBody] SaveRequirementRequestModel request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var requestId = Guid.Parse(_requestContext.TraceId);

        var response = await _requirementAppService.SaveRequirement(
            new SaveRequirementRequest(
            requestId, 
            Guid.Parse(request.projectId),
            request.requirementId == string.Empty ? null : Guid.Parse(request.requirementId),
            request.description))
            .ConfigureAwait(false);

        return response.Success ? Ok(response) : BadRequest(response);
    }
}
