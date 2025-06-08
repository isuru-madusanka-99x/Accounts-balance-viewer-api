using AccountsBalanceViewerAPI.Application.Interfaces;
using AccountsBalanceViewerAPI.Domain.Models;
using AccountsBalanceViewerAPI.Infrastructure.Interfaces;

namespace AccountsBalanceViewerAPI.Application.Services.Accounts;

public class AccountService : IAccountService
{
    private readonly IUnitOfWork _unitOfWork;

    public AccountService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Account>> GetAllAccountsAsync()
    {
        return await _unitOfWork.AccountRepository.GetAsync();
    }

    public async Task<Account?> GetAccountByIdAsync(Guid id)
    {
        return await _unitOfWork.AccountRepository.GetByIdAsync(id);
    }
}
