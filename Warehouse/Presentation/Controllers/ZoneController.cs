using Application.Features.ZoneFeatures;
using Microsoft.AspNetCore.Mvc;
using Presentation.Controllers.Common;

namespace Presentation.Controllers;

public class ZoneController : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetZone()
    {
        return Ok();
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateZone(CreateZoneCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
}