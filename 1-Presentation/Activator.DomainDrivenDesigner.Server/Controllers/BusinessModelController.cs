using Activator.DomainDrivenDesigner.Application.AppRequests;
using Activator.DomainDrivenDesigner.Application.Services;
using Activator.DomainDrivenDesigner.Domain.Entities;
using Activator.DomainDrivenDesigner.Server.Models;
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

    [HttpPost("upsert/{requirementId}")]
    public async Task<ActionResult<bool>> UpsertBusinessModel(Guid requirementId, [FromBody] UpsertBusinessModelModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var requestId = Guid.Parse(_requestContext.TraceId);

        var modelId = string.IsNullOrEmpty(model.id) ? Guid.Empty : Guid.Parse(model.id);
        BusinessModel businessModel = new BusinessModel(modelId)
        { 
            Name = model.name,
            RawDescription = model.rawDescription,
            Properties = model.properties ?? new List<BusinessModelProperty>()
        };

        var response = await _requirementAppService.UpsertProjectBusinessModels(
            new UpsertBusinessModelsAppRequest(requestId, requirementId, businessModel))
            .ConfigureAwait(false);

        return response.Success ? Ok(response) : BadRequest(response);
    }

    [HttpGet("retrieve/{requirementId}/{modelName}")]
    public async Task<ActionResult> RetrieveBusinessModelByName(Guid requirementId, string modelName)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var requestId = Guid.Parse(_requestContext.TraceId);

        var response = await _requirementAppService.RetrieveBusinessModelByName(
            new RetrieveBusinessModelsByNameAppRequest(requestId, requirementId, modelName))
            .ConfigureAwait(false);

        return response.Success ? Ok(response.BusinessModel) : BadRequest(response);
    }
}
