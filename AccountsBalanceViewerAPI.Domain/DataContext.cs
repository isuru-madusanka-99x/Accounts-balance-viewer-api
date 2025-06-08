using AccountsBalanceViewerAPI.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AccountsBalanceViewerAPI.Domain;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    public DbSet<Account> Accounts { get; set; } = null!;
    public DbSet<Balance> Balances { get; set; } = null!;
}
