using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Transactions.Public;

namespace Api.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class PaymentsController : ControllerBase
{

    public PaymentsController()
    {
    }
    
    [HttpPost]
    public async Task<IActionResult> AddPayment()
    {
        var authId = GetAuthId();
        var result = "Add payment!";

        return Ok(result);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllPayments()
    {
        var authId = GetAuthId();
        var result = "Get all payments";

        return Ok(result);
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdatePayment()
    {
        var authId = GetAuthId();
        var result = "Update payment!";

        return Ok(result);
    }
    
    [HttpDelete]
    public async Task<IActionResult> DeletePayment()
    {
        var authId = GetAuthId();
        var result = "Delete payment!";

        return Ok(result);
    }
    private string GetAuthId()
    {
        var authId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (authId == null)
            throw new Exception("Auth ID is null");
        
        return authId;
    }

}