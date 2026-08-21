namespace BE.Entities;

public class Bid
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int BidderId { get; set; }
    public decimal Amount { get; set; }
    public DateTime BidTime { get; set; } = DateTime.UtcNow;

    public virtual Product Product { get; set; } = null!;
    public virtual User Bidder { get; set; } = null!;
}
