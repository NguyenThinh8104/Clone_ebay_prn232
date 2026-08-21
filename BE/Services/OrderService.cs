namespace BE.Services;

public interface IOrderService
{
    Task<object?> GetSellerOrdersAsync(int sellerId, object filter);
    Task<object?> GetSellerOrderByIdAsync(int sellerId, int orderId);
    Task<object?> ConfirmOrderAsync(int sellerId, int orderId);
    Task<object?> GetPaymentStatusAsync(int sellerId, int orderId);
}

public class OrderService : IOrderService
{
    public Task<object?> GetSellerOrdersAsync(int sellerId, object filter) => throw new NotImplementedException();
    public Task<object?> GetSellerOrderByIdAsync(int sellerId, int orderId) => throw new NotImplementedException();
    public Task<object?> ConfirmOrderAsync(int sellerId, int orderId) => throw new NotImplementedException();
    public Task<object?> GetPaymentStatusAsync(int sellerId, int orderId) => throw new NotImplementedException();
}
