namespace BE.Entities;

public class ReturnRequest
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public string Reason { get; set; } = string.Empty;
    public string Status { get; set; } = "Requested"; // Requested, Accepted, RefundOffered, Declined, RefundedByReturn, Closed
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public virtual OrderTable Order { get; set; } = null!;
    public virtual ICollection<Dispute> Disputes { get; set; } = new List<Dispute>();
}
