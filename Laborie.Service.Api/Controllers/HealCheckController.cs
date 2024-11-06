using Microsoft.AspNetCore.Mvc;

namespace Laborie.Service.Api.Controllers;
/// <summary>
/// Healcheck controller
/// </summary>
[Route("/")]
public class HealCheckController : ControllerBase
{
    [HttpGet]
    public IActionResult Index() => Ok("success");
}
