using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountsBalanceViewerAPI.Domain.Models;

[Index(nameof(AccountId), nameof(Year), nameof(Month), IsUnique = true)]
public class Balance : BaseEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid AccountId { get; set; }

    [Required]
    public int Year { get; set; }

    [Required]
    public int Month { get; set; }

    [Required]
    [Precision(18, 2)]
    public decimal Amount { get; set; }

    [ForeignKey(nameof(AccountId))]
    public Account Account { get; set; } = null!;
}