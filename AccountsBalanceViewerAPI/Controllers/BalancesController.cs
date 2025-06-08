using AccountsBalanceViewerAPI.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccountsBalanceViewerAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BalancesController : ControllerBase
{
    private readonly ILogger<BalancesController> _logger;
    private readonly IFileUploadService _fileUploadService;

    public BalancesController(ILogger<BalancesController> logger, IFileUploadService fileUploadService)
    {
        _logger = logger;
        _fileUploadService = fileUploadService;
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

    [HttpPost("upload")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> UploadBalances(IFormFile file)
    {
        var (success, error) = await _fileUploadService.ProcessBalanceFileAsync(file);
        if (!success)
            return BadRequest(new { Message = error });

        return Ok(new { Message = "Balances uploaded successfully" });
    }
}