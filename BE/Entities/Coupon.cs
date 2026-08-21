namespace BE.Entities;

public class Coupon
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int SellerId { get; set; }
    public string Code { get; set; } = string.Empty;
    public decimal DiscountPercent { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int MaxUsage { get; set; } = 1;
    public int UsedCount { get; set; } = 0;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual Product Product { get; set; } = null!;
    public virtual User Seller { get; set; } = null!;
}
