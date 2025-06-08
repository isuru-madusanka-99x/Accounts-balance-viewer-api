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
    private readonly IBalanceService _balanceService;

    public BalancesController(
        ILogger<BalancesController> logger, 
        IFileUploadService fileUploadService,
        IBalanceService balanceService)
    {
        _logger = logger;
        _fileUploadService = fileUploadService;
        _balanceService = balanceService;
    }

    // Get all balance periods (year and month combinations)
    [HttpGet("periods")]
    [Authorize(Policy = "AllUsers")]
    public async Task<IActionResult> GetBalancePeriods()
    {
        try
        {
            var periods = await _balanceService.GetBalancesByPeriodAsync();
            return Ok(new
            {
                success = true,
                message = "Balance periods retrieved successfully.",
                data = periods
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving balance periods");
            return StatusCode(500, new
            {
                success = false,
                message = "An error occurred while retrieving balance periods."
            });
        }
    }

    [HttpPost("upload")]
    [Authorize(Policy = "AdminOnly")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadBalances(IFormFile file)
    {
        var (success, error) = await _fileUploadService.ProcessBalanceFileAsync(file);
        if (!success)
            return BadRequest(new { success = false, message = error });

        return Ok(new { success = true, message = "Balances uploaded successfully." });
    }
}