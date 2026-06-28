using Activator.DomainDrivenDesigner.Application.AppRequests;
using Activator.DomainDrivenDesigner.Application.Services;
using Activator.DomainDrivenDesigner.Domain.Entities;
using Common.Core.AOP.LogTrace;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Activator.DomainDrivenDesigner.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
[EnableCors("AllowDDDClientPolicy")]
public class BusinessModelController(RequirementAppService requirementAppService, IRequestContext requestContext) : ControllerBase
{
    private readonly IRequestContext _requestContext = requestContext;

    private readonly RequirementAppService  _requirementAppService = requirementAppService;

    [HttpPost("model/upsert")]
    public async Task<ActionResult<bool>> UpsertBusinessModel([FromQuery] Guid reqId, [FromBody] BusinessModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var requestId = Guid.Parse(_requestContext.TraceId);

        var response = await _requirementAppService.UpsertProjectBusinessModels(
            new UpsertBusinessModelsAppRequest(requestId, reqId, model))
            .ConfigureAwait(false);

        return response.Success ? Ok(response) : BadRequest(response);
    }
}
