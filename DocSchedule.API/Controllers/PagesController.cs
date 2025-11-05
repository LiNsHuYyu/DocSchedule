using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DocSchedule.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PagesController : ControllerBase
{
    [HttpGet("manager")]
    [Authorize(Policy = "AdminOnly")]
    public IActionResult Manager() => Ok("This is manager page");

    [HttpGet("doctor")]
    [Authorize(Policy = "DoctorOnly")]
    public IActionResult Doctor() => Ok("This is doctor page");
}