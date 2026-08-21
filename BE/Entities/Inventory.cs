namespace BE.Entities;

public class Inventory
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; } = 0;
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

    public virtual Product Product { get; set; } = null!;
}
