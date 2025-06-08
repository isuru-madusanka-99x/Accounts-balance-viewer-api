using AccountsBalanceViewerAPI.Application.Interfaces;
using AccountsBalanceViewerAPI.Application.Models.ViewModels;
using AccountsBalanceViewerAPI.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AccountsBalanceViewerAPI.Application.Services.Balances;

public class BalanceService : IBalanceService
{
    private readonly IUnitOfWork UnitOfWork;

    public BalanceService(IUnitOfWork unitOfWork)
    {
        UnitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<BalancePeriodDto>> GetBalancesByPeriodAsync()
    {
        // Get all balances with their accounts
        var balances = await UnitOfWork.BalanceRepository.GetAsync(
            include: query => query.Include(b => b.Account)
        );

        // Group by year and month
        var result = balances
            .GroupBy(b => new { b.Year, b.Month })
            .OrderByDescending(g => g.Key.Year)
            .ThenByDescending(g => g.Key.Month)
            .Select(g => new BalancePeriodDto
            {
                Year = g.Key.Year,
                Month = g.Key.Month,
                Balances = g.Select(b => new AccountBalanceDto
                {
                    AccountName = b.Account.AccountName,
                    AccountCode = b.Account.AccountCode,
                    Amount = b.Amount
                }).ToList()
            });

        return result;
    }
}