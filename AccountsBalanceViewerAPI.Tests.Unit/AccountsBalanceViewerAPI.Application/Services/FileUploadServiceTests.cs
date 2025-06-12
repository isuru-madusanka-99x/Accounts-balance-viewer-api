using AccountsBalanceViewerAPI.Application.Services.FileUploads;
using AccountsBalanceViewerAPI.Domain.Models;
using AccountsBalanceViewerAPI.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using FakeItEasy;
using Shouldly;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using _4Subsea.ValveTrack.DAL.Interfaces;

namespace AccountsBalanceViewerAPI.Tests.Unit.AccountsBalanceViewerAPI.Application.Services.FileUploads;

public class FileUploadServiceTests
{
    [Fact]
    public async Task ProcessBalanceFileAsync_WithNullFile_ReturnsError()
    {
        // Arrange
        var uow = A.Fake<IUnitOfWork>();
        var service = new FileUploadService(uow);

        // Act
        var result = await service.ProcessBalanceFileAsync(null);

        // Assert
        result.Success.ShouldBeFalse();
        result.ErrorMessage.ShouldBe("File is empty.");
    }

    [Fact]
    public async Task ProcessBalanceFileAsync_WithEmptyFile_ReturnsError()
    {
        // Arrange
        var uow = A.Fake<IUnitOfWork>();
        var fakeFile = A.Fake<IFormFile>();
        A.CallTo(() => fakeFile.Length).Returns(0);
        var service = new FileUploadService(uow);

        // Act
        var result = await service.ProcessBalanceFileAsync(fakeFile);

        // Assert
        result.Success.ShouldBeFalse();
        result.ErrorMessage.ShouldBe("File is empty.");
    }

    [Fact]
    public async Task ProcessBalanceFileAsync_WithUnsupportedFileType_ReturnsError()
    {
        // Arrange
        var uow = A.Fake<IUnitOfWork>();
        var fakeFile = A.Fake<IFormFile>();
        A.CallTo(() => fakeFile.Length).Returns(10);
        A.CallTo(() => fakeFile.FileName).Returns("file.unsupported");
        A.CallTo(() => uow.AccountRepository.GetAsync(null, null, null, false)).Returns(new List<Account>());
        var service = new FileUploadService(uow);

        // Act
        var result = await service.ProcessBalanceFileAsync(fakeFile);

        // Assert
        result.Success.ShouldBeFalse();
        result.ErrorMessage.ShouldBe("Unsupported file type.");
    }

    [Fact]
    public async Task ProcessBalanceFileAsync_WithValidTxtFile_ProcessesAndReplacesBalances()
    {
        // Arrange
        var uow = A.Fake<IUnitOfWork>();
        var account = new Account { Id = Guid.NewGuid(), AccountName = "Cash", AccountCode = "1000" };
        A.CallTo(() => uow.AccountRepository.GetAsync(null, null, null, false)).Returns(new List<Account> { account });

        var txtContent = "for January 2024\nCash 123.45\n";
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(txtContent));
        var fakeFile = A.Fake<IFormFile>();
        A.CallTo(() => fakeFile.Length).Returns(stream.Length);
        A.CallTo(() => fakeFile.FileName).Returns("balances.txt");
        A.CallTo(() => fakeFile.OpenReadStream()).Returns(stream);

        var fakeBalanceRepo = A.Fake<IGenericRepository<Balance>>();
        A.CallTo(() => uow.BalanceRepository).Returns(fakeBalanceRepo);
        A.CallTo(() => fakeBalanceRepo.GetAsync(A<System.Linq.Expressions.Expression<Func<Balance, bool>>>._, null, null, false))
            .Returns(new List<Balance>());

        var service = new FileUploadService(uow);

        // Act
        var result = await service.ProcessBalanceFileAsync(fakeFile);

        // Assert
        result.Success.ShouldBeTrue();
        result.ErrorMessage.ShouldBeNull();
        A.CallTo(() => fakeBalanceRepo.InsertAsync(A<Balance>.That.Matches(b =>
            b.AccountId == account.Id &&
            b.Amount == 123.45m &&
            b.Year == 2024 &&
            b.Month == 1
        ))).MustHaveHappenedOnceExactly();
        A.CallTo(() => uow.SaveAsync(default)).MustHaveHappenedOnceExactly();
    }
}
