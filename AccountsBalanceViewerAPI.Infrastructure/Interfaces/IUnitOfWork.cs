
using AccountsBalanceViewerAPI.Domain;

namespace AccountsBalanceViewerAPI.Infrastructure.Interfaces;

public interface IUnitOfWork : IDisposable
{
    DataContext Db { get; }
    //IPlanRepository PlanRepository { get; }
    //IGenericRepository<ValidPlanStatus> ValidPlanStatusRepository { get; }
    void Save();
    Task SaveAsync(CancellationToken token);
}
