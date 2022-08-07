﻿using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Payments.Public;
using Transactions.Public;

namespace Api.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentsService _paymentsService;

    public PaymentsController(IPaymentsService paymentsService)
    {
        _paymentsService = paymentsService;
    }
    
    [HttpPost]
    public async Task<IActionResult> AddPayment(ApiAddPaymentInput input)
    {
        var authId = GetAuthId();
        var result = await _paymentsService.AddPayment(authId, input.ToAddPaymentInput());

        return Ok(result);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllPayments()
    {
        var authId = GetAuthId();
        var payments = (await _paymentsService.GetPayments(authId)).Select(x => x.ToApiPayment());

        var result = new
        {
            Payments = payments,
            MonthlyIncoming = new
            {
                Count = 1,
                Total = 7950.54M 
            },
            MonthlyOutgoing = new
            {
                Count = 8,
                Total = 3568M 
            },
            MonthlyNet = new
            {
                Count = 9,
                Total = 4402.54M
            },
            CategoryTotals = new List<object>
            {
                new {Category = "FIXED_COSTS", Total = 3568M},
                new {Category = "INCOME", Total = 7950.54M},
            }
            
        };

        return Ok(result);
    }
    
    [HttpGet]
    [Route("summary")]
    public async Task<IActionResult> GetPaymentSummary(GetPaymentsSummaryInput input)
    {
        var authId = GetAuthId();
        var result = await _paymentsService.GetPaymentsSummary(authId, input);

        return Ok(result);
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdatePayment(UpdatePaymentInput input)
    {
        var authId = GetAuthId();
        var result = await _paymentsService.UpdatePayment(authId, input);

        return Ok(result);
    }
    
    [HttpDelete]
    public async Task<IActionResult> DeletePayment(DeletePaymentInput input)
    {
        var authId = GetAuthId();
        await _paymentsService.DeletePayment(authId, input.PaymentId);

        return Ok("Payment deleted");
    }
    private string GetAuthId()
    {
        var authId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (authId == null)
            throw new Exception("Auth ID is null");
        
        return authId;
    }

}

public record DeletePaymentInput(int PaymentId);