using _4Subsea.ValveTrack.DAL.Interfaces;
using AccountsBalanceViewerAPI.Application.Services.Accounts;
using AccountsBalanceViewerAPI.Domain.Models;
using AccountsBalanceViewerAPI.Infrastructure.Interfaces;
using FakeItEasy;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AccountsBalanceViewerAPI.Tests.Unit.AccountsBalanceViewerAPI.Application.Services.Accounts;

public class AccountServiceTests
{
    [Fact]
    public async Task GetAllAccountsAsync_ReturnsAllAccounts()
    {
        // Arrange
        var fakeUow = A.Fake<IUnitOfWork>();
        var fakeRepo = A.Fake<IGenericRepository<Account>>();
        var accounts = new List<Account>
        {
            new Account { Id = Guid.NewGuid(), AccountName = "R&D", AccountCode = "RD" },
            new Account { Id = Guid.NewGuid(), AccountName = "Canteen", AccountCode = "CT" }
        };
        A.CallTo(() => fakeRepo.GetAsync(null, null, null, false)).Returns(accounts);
        A.CallTo(() => fakeUow.AccountRepository).Returns(fakeRepo);

        var service = new AccountService(fakeUow);

        // Act
        var result = await service.GetAllAccountsAsync();

        // Assert
        result.Count().ShouldBe(2);
        result.Any(a => a.AccountName == "R&D").ShouldBeTrue();
    }

    [Fact]
    public async Task GetAccountByIdAsync_ReturnsAccount_WhenExists()
    {
        var id = Guid.NewGuid();
        var account = new Account { Id = id, AccountName = "R&D", AccountCode = "RD" };
        var fakeUow = A.Fake<IUnitOfWork>();
        var fakeRepo = A.Fake<IGenericRepository<Account>>();
        A.CallTo(() => fakeRepo.GetByIdAsync(id)).Returns(account);
        A.CallTo(() => fakeUow.AccountRepository).Returns(fakeRepo);

        var service = new AccountService(fakeUow);

        var result = await service.GetAccountByIdAsync(id);

        result.ShouldNotBeNull();
        result!.AccountName.ShouldBe("R&D");
    }

    [Fact]
    public async Task GetAccountByIdAsync_ReturnsNull_WhenNotExists()
    {
        var id = Guid.NewGuid();
        var fakeUow = A.Fake<IUnitOfWork>();
        var fakeRepo = A.Fake<IGenericRepository<Account>>();
        A.CallTo(() => fakeRepo.GetByIdAsync(id)).Returns((Account?)null);
        A.CallTo(() => fakeUow.AccountRepository).Returns(fakeRepo);

        var service = new AccountService(fakeUow);

        var result = await service.GetAccountByIdAsync(id);

        result.ShouldBeNull();
    }
}
