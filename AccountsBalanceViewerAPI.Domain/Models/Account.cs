using AccountsBalanceViewerAPI.Domain.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace AccountsBalanceViewerAPI.Domain.Models;

public class Account : BaseEntity, IWithId
{
    [Key]
    public Guid Id { get; set; }
    public required string AccountName { get; set; }
    public required string AccountCode { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
}
