using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccountsBalanceViewerAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BalancesController : ControllerBase
{
    private readonly ILogger<BalancesController> _logger;

    public BalancesController(ILogger<BalancesController> logger)
    {
        _logger = logger;
    }

    // Endpoint accessible by all authenticated users
    [HttpGet]
    [Authorize(Policy = "AllUsers")]
    public IActionResult GetBalances()
    {
        // Return account balances data
        return Ok(new
        {
            Message = "Current balances retrieved successfully",
            Accounts = new[] {
                new { AccountId = "ACC001", Balance = 1250.50, Name = "Checking Account" },
                new { AccountId = "ACC002", Balance = 8750.75, Name = "Savings Account" }
            }
        });
    }

    // Endpoint accessible only by admin users
    [HttpPost("upload")]
    [Authorize(Policy = "AdminOnly")]
    public IActionResult UploadBalances([FromBody] object balanceData)
    {
        // Process balance upload (admin only feature)
        return Ok(new { Message = "Balances uploaded successfully" });
    }

    // Endpoint accessible only by admin users
    [HttpGet("reports")]
    [Authorize(Policy = "AdminOnly")]
    public IActionResult GetReports()
    {
        // Return admin reports
        return Ok(new
        {
            Message = "Reports retrieved successfully",
            Reports = new[] {
                new { ReportId = "REP001", Name = "Monthly Balance Summary", GeneratedDate = DateTime.Now }
            }
        });
    }
}