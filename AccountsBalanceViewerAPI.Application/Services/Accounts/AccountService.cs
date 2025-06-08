using AccountsBalanceViewerAPI.Application.Interfaces;
using AccountsBalanceViewerAPI.Domain.Models;
using AccountsBalanceViewerAPI.Infrastructure.Interfaces;

namespace AccountsBalanceViewerAPI.Application.Services.Accounts;

public class AccountService : IAccountService
{
    private readonly IUnitOfWork UnitOfWork;

    public AccountService(IUnitOfWork unitOfWork)
    {
        UnitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Account>> GetAllAccountsAsync()
    {
        return await UnitOfWork.AccountRepository.GetAsync();
    }

    public async Task<Account?> GetAccountByIdAsync(Guid id)
    {
        return await UnitOfWork.AccountRepository.GetByIdAsync(id);
    }
}
