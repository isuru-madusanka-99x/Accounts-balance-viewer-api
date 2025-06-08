using Microsoft.AspNetCore.Http;

namespace AccountsBalanceViewerAPI.Application.Interfaces;

public interface IFileUploadService
{
    Task<(bool Success, string? ErrorMessage)> ProcessBalanceFileAsync(IFormFile file);
}
