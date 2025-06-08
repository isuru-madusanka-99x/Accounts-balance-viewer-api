using _4Subsea.ValveTrack.DAL.Interfaces;
using AccountsBalanceViewerAPI.Domain;
using AccountsBalanceViewerAPI.Domain.Models;
using AccountsBalanceViewerAPI.Infrastructure.Interfaces;

namespace AccountsBalanceViewerAPI.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly DataContext _context;

    private GenericRepository<Account>? _accountRepository;
    private GenericRepository<Balance>? _balanceRepository;

    public DataContext Db => _context;

    public UnitOfWork(DataContext applicationDbContext)
    {
        _context = applicationDbContext;
    }

    public IGenericRepository<Account> AccountRepository
    {
        get
        {
            if (_accountRepository == null)
            {
                _accountRepository = new GenericRepository<Account>(_context);
            }
            return _accountRepository;
        }
    }

    public IGenericRepository<Balance> BalanceRepository
    {
        get
        {
            if (_balanceRepository == null)
            {
                _balanceRepository = new GenericRepository<Balance>(_context);
            }
            return _balanceRepository;
        }
    }

    public void Save()
    {
        _context.SaveChanges();
    }

    public async Task SaveAsync(CancellationToken token)
    {
        await _context.SaveChangesAsync(token);
    }

    private bool disposed = false;

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
        disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}

