namespace AccountsBalanceViewerAPI.Application.Models.ViewModels;

public record BalancePeriodDto
{
    public int Year { get; init; }
    public int Month { get; init; }
    public string MonthName => System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Month);
    public List<AccountBalanceDto> Balances { get; init; } = new();
}