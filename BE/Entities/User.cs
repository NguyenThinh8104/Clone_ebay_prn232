namespace BE.Entities;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Role { get; set; } = "Buyer"; // Buyer, Seller, Admin
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual Store? Store { get; set; }
    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    public virtual ICollection<OrderTable> Orders { get; set; } = new List<OrderTable>();
    public virtual ICollection<Bid> Bids { get; set; } = new List<Bid>();
    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    public virtual Feedback? Feedback { get; set; }
}
