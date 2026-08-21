namespace BE.Entities;

public class Review
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int BuyerId { get; set; }
    public int Rating { get; set; } // 1 to 5
    public string? Comment { get; set; }
    public string? Response { get; set; } // Seller reply (max 1 time)
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual Product Product { get; set; } = null!;
    public virtual User Buyer { get; set; } = null!;
}
