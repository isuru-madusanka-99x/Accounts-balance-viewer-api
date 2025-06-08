namespace AccountsBalanceViewerAPI.Application.Models.ViewModels;

public record AccountBalanceDto
{
    public string AccountName { get; init; } = string.Empty;
    public string AccountCode { get; init; } = string.Empty;
    public decimal Amount { get; init; }
    public string FormattedAmount => string.Format("Rs. {0:N2}/=", Amount);
}
