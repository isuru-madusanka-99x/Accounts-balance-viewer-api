
using _4Subsea.ValveTrack.DAL.Interfaces;
using AccountsBalanceViewerAPI.Domain;
using AccountsBalanceViewerAPI.Domain.Models;

namespace AccountsBalanceViewerAPI.Infrastructure.Interfaces;

public interface IUnitOfWork : IDisposable
{
    DataContext Db { get; }
    IGenericRepository<Account> AccountRepository { get; }
    IGenericRepository<Balance> BalanceRepository { get; }
    void Save();
    Task SaveAsync(CancellationToken token);
}
