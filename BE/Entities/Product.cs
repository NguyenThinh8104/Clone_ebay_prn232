namespace BE.Entities;

public class Product
{
    public int Id { get; set; }
    public int SellerId { get; set; }
    public int CategoryId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public bool IsAuction { get; set; } = false;
    public DateTime? AuctionEndTime { get; set; }
    public string? Images { get; set; } // JSON String Array
    public string Status { get; set; } = "Active"; // Active, Hidden, OutOfStock, AuctionEnded
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public virtual User Seller { get; set; } = null!;
    public virtual Category Category { get; set; } = null!;
    public virtual Inventory? Inventory { get; set; }
    public virtual ICollection<Coupon> Coupons { get; set; } = new List<Coupon>();
    public virtual ICollection<Bid> Bids { get; set; } = new List<Bid>();
    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
