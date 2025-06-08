using AccountsBalanceViewerAPI.Application.Models.ViewModels;

namespace AccountsBalanceViewerAPI.Application.Interfaces;

public interface IBalanceService
{
    Task<IEnumerable<BalancePeriodDto>> GetBalancesByPeriodAsync();
}
