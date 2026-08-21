namespace BE.Entities;

public class Dispute
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int? ReturnRequestId { get; set; }
    public int BuyerId { get; set; }
    public int SellerId { get; set; }
    public string Reason { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Status { get; set; } = "Open"; // Open, UnderReview, Resolved
    public string? Resolution { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public virtual OrderTable Order { get; set; } = null!;
    public virtual ReturnRequest? ReturnRequest { get; set; }
    public virtual User Buyer { get; set; } = null!;
    public virtual User Seller { get; set; } = null!;
}
