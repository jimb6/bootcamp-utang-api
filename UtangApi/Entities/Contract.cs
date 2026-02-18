using UtangApp.Api.Enums;

namespace UtangApp.Api.Entities;

public class Contract
{
    public int Id { get; set; }
    public int BorrowerId { get; set; }
    public decimal PrincipalAmount { get; set; }
    public decimal InterestRate { get; set; }
    public InterestMode InterestMode { get; set; }
    public TermType TermType { get; set; }
    public int TermCount { get; set; }
    public decimal LiquidationRate { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal RemainingBalance { get; set; }
    public decimal AmountPerTerm { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly DueDate { get; set; }
    public ContractStatus Status { get; set; } = ContractStatus.Active;
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public Borrower Borrower { get; set; } = null!;
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
