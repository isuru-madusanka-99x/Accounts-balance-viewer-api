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

namespace AccountsBalanceViewerAPI.Tests.Unit.AccountsBalanceViewerAPI.Application.Services.FileUploads;

public class FileUploadServiceTests
{
    /*
    [Fact]
    public async Task ProcessBalanceFileAsync_ReturnsFalse_WhenFileIsEmpty()
    {
        var fakeUow = A.Fake<IUnitOfWork>();
        var service = new FileUploadService(fakeUow);

        var fakeFile = A.Fake<IFormFile>();
        A.CallTo(() => fakeFile.Length).Returns(0);

        var (success, error) = await service.ProcessBalanceFileAsync(fakeFile);

        success.ShouldBeFalse();
        error.ShouldBe("File is empty.");
    }

    [Fact]
    public async Task ProcessBalanceFileAsync_ReturnsFalse_WhenUnsupportedFileType()
    {
        var fakeUow = A.Fake<IUnitOfWork>();
        var service = new FileUploadService(fakeUow);

        var fakeFile = A.Fake<IFormFile>();
        A.CallTo(() => fakeFile.Length).Returns(10);
        A.CallTo(() => fakeFile.FileName).Returns("test.unsupported");

        var (success, error) = await service.ProcessBalanceFileAsync(fakeFile);

        success.ShouldBeFalse();
        error.ShouldBe("Unsupported file type.");
    }
    */
    // For more advanced tests, you can create a MemoryStream with valid .txt or .xlsx content and set up the IFormFile accordingly.
}
