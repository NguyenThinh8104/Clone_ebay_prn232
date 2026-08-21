namespace BE.Entities;

public class ShippingInfo
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public string? Carrier { get; set; } // GHN, GHTK, ViettelPost
    public string? TrackingNumber { get; set; }
    public string Status { get; set; } = "Preparing"; // Preparing, LabelCreated, HandedToCarrier, InTransit, Delivered
    public DateTime? EstimatedArrival { get; set; }
    public DateTime? ShippedAt { get; set; }
    public DateTime? DeliveredAt { get; set; }

    public virtual OrderTable Order { get; set; } = null!;
}
