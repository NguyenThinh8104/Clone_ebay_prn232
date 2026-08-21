namespace BE.Entities;

public class OrderItem
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int? CouponId { get; set; }
    public int Quantity { get; set; } = 1;
    public decimal UnitPrice { get; set; }

    public virtual OrderTable Order { get; set; } = null!;
    public virtual Product Product { get; set; } = null!;
    public virtual Coupon? Coupon { get; set; }
}
