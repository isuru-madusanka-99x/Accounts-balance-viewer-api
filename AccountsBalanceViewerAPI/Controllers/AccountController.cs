using AccountsBalanceViewerAPI.Application.Interfaces;
using AccountsBalanceViewerAPI.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccountsBalanceViewerAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize("AllUsers")]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;
    private readonly ILogger<AccountController> _logger;

    public AccountController(IAccountService accountService, ILogger<AccountController> logger)
    {
        _accountService = accountService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAccounts()
    {
        try
        {
            var accounts = await _accountService.GetAllAccountsAsync();
            return Ok(new
            {
                success = true,
                message = "Accounts retrieved successfully.",
                data = accounts
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving accounts");
            return StatusCode(500, new
            {
                success = false,
                message = "An error occurred while retrieving accounts."
            });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAccount(Guid id)
    {
        try
        {
            var account = await _accountService.GetAccountByIdAsync(id);
            if (account == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = $"Account with ID {id} not found."
                });
            }
            return Ok(new
            {
                success = true,
                message = "Account retrieved successfully.",
                data = account
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving account with ID {Id}", id);
            return StatusCode(500, new
            {
                success = false,
                message = $"An error occurred while retrieving account with ID {id}."
            });
        }
    }
}