
using _4Subsea.ValveTrack.DAL.Interfaces;
using AccountsBalanceViewerAPI.Domain;
using AccountsBalanceViewerAPI.Domain.Models;

namespace AccountsBalanceViewerAPI.Infrastructure.Interfaces;

public interface IUnitOfWork : IDisposable
{
    DataContext Db { get; }
    //IPlanRepository PlanRepository { get; }
    IGenericRepository<Account> AccountRepository { get; }
    void Save();
    Task SaveAsync(CancellationToken token);
}
