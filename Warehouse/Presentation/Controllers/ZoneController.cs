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
    public async Task<IActionResult> PostZone()
    {
        return Ok();
    }
}