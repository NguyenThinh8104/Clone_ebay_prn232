namespace BE.Entities;

public class SalesSummary
{
    public int Id { get; set; }
    public int SellerId { get; set; }
    public string Period { get; set; } = "week"; // week, month
    public decimal TotalRevenue { get; set; } = 0;
    public int TotalOrders { get; set; } = 0;
    public decimal AverageOrderValue { get; set; } = 0;
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

    public virtual User Seller { get; set; } = null!;
}
