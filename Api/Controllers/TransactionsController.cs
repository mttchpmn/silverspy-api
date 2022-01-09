using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Transactions.Public;

namespace Api.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class TransactionsController : ControllerBase
{
    private readonly ITransactionsService _transactionsService;

    public TransactionsController(ITransactionsService transactionsService)
    {
        _transactionsService = transactionsService;
    }
    
    [HttpPost]
    public async Task<IActionResult> Import(ImportTransactionsInput input)
    {
        var authId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var result = await _transactionsService.ImportTransactions(authId!, input);
        
        return Ok(result);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetTransactions()
    {
        var authId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var result = await _transactionsService.GetTransactions(authId!);
        
        return Ok(result);
    }
}