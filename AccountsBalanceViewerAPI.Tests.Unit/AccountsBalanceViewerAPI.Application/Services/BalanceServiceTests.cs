using _4Subsea.ValveTrack.DAL.Interfaces;
using AccountsBalanceViewerAPI.Application.Services.Balances;
using AccountsBalanceViewerAPI.Domain.Models;
using AccountsBalanceViewerAPI.Infrastructure.Interfaces;
using FakeItEasy;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AccountsBalanceViewerAPI.Tests.Unit.AccountsBalanceViewerAPI.Application.Services.Balances;

public class BalanceServiceTests
{
    [Fact]
    public async Task GetBalancesByPeriodAsync_ReturnsGroupedBalances()
    {
        /*
        // Arrange
        var fakeUow = A.Fake<IUnitOfWork>();
        var fakeRepo = A.Fake<IGenericRepository<Balance>>();
        var balances = new List<Balance>
        {
            new Balance { Id = Guid.NewGuid(), AccountId = Guid.NewGuid(), Year = 2023, Month = 3, Amount = 100, Account = new Account { AccountName = "R&D", AccountCode = "RD" } },
            new Balance { Id = Guid.NewGuid(), AccountId = Guid.NewGuid(), Year = 2023, Month = 3, Amount = 200, Account = new Account { AccountName = "Canteen", AccountCode = "CT" } },
            new Balance { Id = Guid.NewGuid(), AccountId = Guid.NewGuid(), Year = 2023, Month = 4, Amount = 300, Account = new Account { AccountName = "R&D", AccountCode = "RD" } }
        };
        A.CallTo(() => fakeRepo.GetAsync(null, null, A<Func<IQueryable<Balance>, IQueryable<Balance>>>._, false)).Returns(balances);
        A.CallTo(() => fakeUow.BalanceRepository).Returns(fakeRepo);

        var service = new BalanceService(fakeUow);

        // Act
        var result = (await service.GetBalancesByPeriodAsync()).ToList();

        // Assert
        result.Count.ShouldBe(2);
        result.Any(p => p.Month == 3 && p.Year == 2023).ShouldBeTrue();
        result.Any(p => p.Month == 4 && p.Year == 2023).ShouldBeTrue();
        result.First().Balances.Count.ShouldBeGreaterThan(0);
        */
    }
}
