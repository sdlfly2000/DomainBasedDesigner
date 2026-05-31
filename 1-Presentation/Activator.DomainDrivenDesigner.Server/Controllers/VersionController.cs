using Activator.DomainDrivenDesigner.Support.Core.Version;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Activator.DomainDrivenDesigner.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
[EnableCors("AllowDDDClientPolicy")]
public class VersionController : ControllerBase
{
    public IActionResult Index()
    {                       
        return Ok(VersionManger.GetVersion());
    }
}