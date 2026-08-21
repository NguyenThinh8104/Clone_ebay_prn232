namespace BE.Services;

public interface IShippingService
{
    Task<object?> UpdateShippingStatusAsync(int sellerId, int orderId, string status);
    Task<object?> CreateShippingLabelAsync(int sellerId, int orderId, string carrier);
    Task<object?> GetShippingInfoAsync(int sellerId, int orderId);
}

public class ShippingService : IShippingService
{
    public Task<object?> UpdateShippingStatusAsync(int sellerId, int orderId, string status) => throw new NotImplementedException();
    public Task<object?> CreateShippingLabelAsync(int sellerId, int orderId, string carrier) => throw new NotImplementedException();
    public Task<object?> GetShippingInfoAsync(int sellerId, int orderId) => throw new NotImplementedException();
}
