using UtangApp.Api.Enums;

namespace UtangApp.Api.Entities;

public class Offer
{
    public int Id { get; set; }
    public int BorrowerId { get; set; }
    public decimal OfferedAmount { get; set; }
    public decimal InterestRate { get; set; }
    public int TermMonths { get; set; }
    public DateOnly OfferDate { get; set; }
    public DateOnly ExpiryDate { get; set; }
    public OfferStatus Status { get; set; } = OfferStatus.Pending;
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public Borrower Borrower { get; set; } = null!;
}
