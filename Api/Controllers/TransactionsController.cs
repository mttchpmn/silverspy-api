using Microsoft.AspNetCore.Mvc;
using Transactions.Public;

namespace Api.Controllers;

[ApiController]
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
        var result = await _transactionsService.ImportTransactions(input);
        return Ok(result);
    }
}