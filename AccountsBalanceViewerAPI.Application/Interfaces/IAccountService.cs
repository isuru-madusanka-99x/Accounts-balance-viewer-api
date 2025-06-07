using AccountsBalanceViewerAPI.Domain.Models;

namespace AccountsBalanceViewerAPI.Application.Interfaces;

public interface IAccountService
{
    Task<IEnumerable<Account>> GetAllAccountsAsync();
    Task<Account?> GetAccountByIdAsync(Guid id);
}
