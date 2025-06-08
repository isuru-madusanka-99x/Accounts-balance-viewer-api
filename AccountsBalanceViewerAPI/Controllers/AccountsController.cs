using AccountsBalanceViewerAPI.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccountsBalanceViewerAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize("AllUsers")]
public class AccountsController : ControllerBase
{
    private readonly IAccountService AccountsService;
    private readonly ILogger<AccountsController> Logger;

    public AccountsController(IAccountService accountService, ILogger<AccountsController> logger)
    {
        AccountsService = accountService;
        Logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAccounts()
    {
        try
        {
            var accounts = await AccountsService.GetAllAccountsAsync();
            return Ok(new
            {
                success = true,
                message = "Accounts retrieved successfully.",
                data = accounts
            });
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error retrieving accounts");
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
            var account = await AccountsService.GetAccountByIdAsync(id);
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
            Logger.LogError(ex, "Error retrieving account with ID {Id}", id);
            return StatusCode(500, new
            {
                success = false,
                message = $"An error occurred while retrieving account with ID {id}."
            });
        }
    }
}