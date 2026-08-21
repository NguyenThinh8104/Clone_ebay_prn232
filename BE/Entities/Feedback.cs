namespace BE.Entities;

public class Feedback
{
    public int Id { get; set; }
    public int SellerId { get; set; }
    public decimal AverageRating { get; set; } = 0;
    public int TotalReviews { get; set; } = 0;
    public decimal PositiveRate { get; set; } = 0;
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

    public virtual User Seller { get; set; } = null!;
}
