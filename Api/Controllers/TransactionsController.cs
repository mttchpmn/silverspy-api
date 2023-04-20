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
    private readonly ILogger<TransactionsController> _logger;

    public TransactionsController(ITransactionsService transactionsService, ILogger<TransactionsController> logger)
    {
        _transactionsService = transactionsService;
        _logger = logger;
    }
    
    [HttpPost]
    public async Task<IActionResult> Import(ImportTransactionsInput input)
    {
        _logger.LogInformation("GLOG: Importing transactions for Bank type: {BankType}", input.BankType);
        Console.WriteLine($"CONSOLE: Importing transactions for Bank type: {input.BankType}");
        
        var authId = GetAuthId();
        var result = await _transactionsService.ImportTransactions(authId!, input);
        
        return Ok(result);
    }
    
    [HttpPost]
    [Route("ingest")]
    public async Task<IActionResult> Ingest(IngestTransactionsInput input)
    {
        _logger.LogInformation("Ingesting transactions");
        
        var authId = GetAuthId();
        var result = await _transactionsService.IngestTransactions(authId!, input);
        
        return Ok(result);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetTransactions()
    {
        var authId = GetAuthId();

        var transactionData = await _transactionsService.GetTransactionData(authId);
        
        return Ok(transactionData);
    }

    [HttpPatch]
    public async Task<IActionResult> UpdateTransaction(UpdateTransactionInput input)
    {
        var authId = GetAuthId();

        var result = await _transactionsService.UpdateTransaction(authId, input);

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