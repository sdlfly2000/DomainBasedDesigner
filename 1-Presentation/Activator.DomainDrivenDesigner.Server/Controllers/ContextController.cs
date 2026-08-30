using Activator.DomainDrivenDesigner.Application.AppRequests;
using Activator.DomainDrivenDesigner.Application.Services;
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

    [HttpGet("retrieve")]
    public async Task<IActionResult> Retrieve()
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var requestId = Guid.Parse(_requestContext.TraceId);

        var response = await _contextAppService.RetrieveContexts(new RetrieveContextAppRequest(requestId)).ConfigureAwait(false);
        
        return response.Success ? Ok(response) : Problem(response.ErrorMessage, statusCode: 500);
    }
}