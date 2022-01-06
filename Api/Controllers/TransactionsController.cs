using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class TransactionsController : ControllerBase
{
    [HttpPost]
    public IActionResult Import()
    {
        return Ok("Imported successfully");
    }
}