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
        // TODO: Replace with actual data retrieval logic
        return Ok(new
        {
            success = true,
            message = "Current balances retrieved successfully.",
            data = new[]
            {
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
        // TODO: Replace with actual report retrieval logic
        return Ok(new
        {
            success = true,
            message = "Reports retrieved successfully.",
            data = new[]
            {
                new { ReportId = "REP001", Name = "Monthly Balance Summary", GeneratedDate = DateTime.Now }
            }
        });
    }

    [HttpPost("upload")]
    [Authorize(Policy = "AdminOnly")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadBalances([FromForm] IFormFile file)
    {
        var (success, error) = await _fileUploadService.ProcessBalanceFileAsync(file);
        if (!success)
            return BadRequest(new { success = false, message = error });

        return Ok(new { success = true, message = "Balances uploaded successfully." });
    }
}