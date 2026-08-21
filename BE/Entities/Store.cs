namespace BE.Entities;

public class Store
{
    public int Id { get; set; }
    public int SellerId { get; set; }
    public string StoreName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? BannerImageURL { get; set; }
    public string SellerType { get; set; } = "Individual"; // Individual, Business
    public string? LegalName { get; set; }
    public string? Phone { get; set; }
    public string VerificationStatus { get; set; } = "Pending"; // Pending, Approved, Rejected
    public string? RejectionReason { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public virtual User Seller { get; set; } = null!;
}
