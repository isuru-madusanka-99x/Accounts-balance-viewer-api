using AccountsBalanceViewerAPI.Application.Interfaces;
using AccountsBalanceViewerAPI.Domain.Models;
using AccountsBalanceViewerAPI.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using ClosedXML.Excel;
using System.Globalization;
using System.Text.RegularExpressions;

namespace AccountsBalanceViewerAPI.Application.Services.FileUploads;

public class FileUploadService : IFileUploadService
{
    private readonly IUnitOfWork UnitOfWork;
    private readonly Regex HeaderWithMonthYearRegex = new(@"for\s+([A-Za-z]+)\s+(\d{4})", RegexOptions.IgnoreCase | RegexOptions.Compiled);
    private readonly Regex AccountAmountRegex = new(@"^(.*?)(-?\d+(\.\d{1,2})?)$", RegexOptions.Compiled);

    public FileUploadService(IUnitOfWork unitOfWork)
    {
        UnitOfWork = unitOfWork;
    }

    public async Task<(bool Success, string? ErrorMessage)> ProcessBalanceFileAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return (false, "File is empty.");

        var accounts = (await UnitOfWork.AccountRepository.GetAsync()).ToList();
        var balances = new List<Balance>();
        int year = 0, month = 0;

        try
        {
            if (file.FileName.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                using var stream = file.OpenReadStream();
                using var workbook = new XLWorkbook(stream);
                var worksheet = workbook.Worksheets.First();

                var lastRowUsed = worksheet.LastRowUsed();
                if (lastRowUsed == null)
                    return (false, "The worksheet does not contain any used rows.");

                var monthText = worksheet.Cell(1, 2).GetString().Trim();
                var yearText = worksheet.Cell(1, 3).GetString().Trim();
                if (!int.TryParse(yearText, out year) || string.IsNullOrWhiteSpace(monthText))
                    return (false, "Could not determine year and month from Excel header.");
                try
                {
                    month = DateTime.ParseExact(monthText, "MMMM", CultureInfo.InvariantCulture).Month;
                }
                catch
                {
                    return (false, $"Invalid month name '{monthText}' in Excel header.");
                }

                for (int row = 4; row <= (lastRowUsed.RowNumber() - 1); row++)
                {
                    var name = worksheet.Cell(row, 1).GetString().Trim();
                    var amountText = worksheet.Cell(row, 2).GetString().Replace(",", "").Trim();
                    if (string.IsNullOrWhiteSpace(name)) continue;
                    if (!decimal.TryParse(amountText, NumberStyles.Any, CultureInfo.InvariantCulture, out var amount))
                        return (false, $"Invalid amount at row {row}.");

                    var account = accounts.FirstOrDefault(a => a.AccountName.Equals(name, StringComparison.OrdinalIgnoreCase));
                    if (account == null)
                        return (false, $"Account '{name}' not found at row {row}.");

                    balances.Add(new Balance
                    {
                        Id = Guid.NewGuid(),
                        AccountId = account.Id,
                        Amount = amount,
                        Year = year,
                        Month = month
                    });
                }
            }
            else if (file.FileName.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
            {
                using var reader = new StreamReader(file.OpenReadStream());
                string? headerLine = await reader.ReadLineAsync();
                var yearMonth = ParseYearMonthFromHeader(headerLine ?? "");
                if (yearMonth == null)
                    return (false, "Could not determine year and month from text file header.");
                year = yearMonth.Value.year;
                month = yearMonth.Value.month;

                string? line;
                int row = 1;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    row++;
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    var accountAndAmount = ExtractAccountAndAmount(line);
                    if (accountAndAmount == null)
                        return (false, $"Invalid format at row {row}.");
                    var (accountName, amount) = accountAndAmount.Value;

                    var account = accounts.FirstOrDefault(a => a.AccountName.Equals(accountName, StringComparison.OrdinalIgnoreCase));
                    if (account == null)
                        return (false, $"Account '{accountName}' not found at row {row}.");

                    balances.Add(new Balance
                    {
                        Id = Guid.NewGuid(),
                        AccountId = account.Id,
                        Amount = amount,
                        Year = year,
                        Month = month
                    });
                }
            }
            else
            {
                return (false, "Unsupported file type.");
            }

            // Remove existing balances for the same year and month before inserting new ones
            var existing = await UnitOfWork.BalanceRepository.GetAsync(
                filter: b => b.Year == year && b.Month == month
            );
            foreach (var balance in existing)
            {
                UnitOfWork.BalanceRepository.Delete(balance);
            }

            foreach (var balance in balances)
            {
                await UnitOfWork.BalanceRepository.InsertAsync(balance);
            }

            await UnitOfWork.SaveAsync(default);

            return (true, null);
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }
    }

    private (int year, int month)? ParseYearMonthFromHeader(string header)
    {
        var match = HeaderWithMonthYearRegex.Match(header);
        if (match.Success)
        {
            var monthName = match.Groups[1].Value;
            var year = int.Parse(match.Groups[2].Value);
            var month = DateTime.ParseExact(monthName, "MMMM", CultureInfo.InvariantCulture).Month;
            return (year, month);
        }
        return null;
    }

    private (string AccountName, decimal Amount)? ExtractAccountAndAmount(string input)
    {
        var match = AccountAmountRegex.Match(input);
        if (match.Success)
        {
            var accountName = match.Groups[1].Value.Trim();
            if (decimal.TryParse(match.Groups[2].Value, NumberStyles.Any, CultureInfo.InvariantCulture, out var amount))
            {
                return (accountName, amount);
            }
        }
        return null;
    }
}