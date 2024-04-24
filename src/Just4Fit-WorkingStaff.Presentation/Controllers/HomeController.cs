namespace Just4Fit_WorkingStaff.Presentation.Controllers;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]/[action]")]
public class HomeController : ControllerBase
{
    [HttpGet]
    public IActionResult Index()
    {
        return base.Ok();
    }

    [HttpGet]
    public IActionResult AboutUs()
    {
        return base.Ok();
    }
}
