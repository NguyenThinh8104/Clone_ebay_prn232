namespace BE.Entities;

public class Payment
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public decimal Amount { get; set; }
    public string Method { get; set; } = string.Empty; // VNPay, CreditCard, PayPal, COD
    public string Status { get; set; } = "Pending"; // Pending, Completed, Failed, Refunded
    public DateTime? PaidAt { get; set; }

    public virtual OrderTable Order { get; set; } = null!;
}
