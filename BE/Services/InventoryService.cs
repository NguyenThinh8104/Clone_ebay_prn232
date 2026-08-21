namespace BE.Services;

public interface IInventoryService
{
    Task<object?> GetInventoryAsync(int sellerId, int productId);
    Task<object?> UpdateInventoryAsync(int sellerId, int productId, int quantity);
}

public class InventoryService : IInventoryService
{
    public Task<object?> GetInventoryAsync(int sellerId, int productId) => throw new NotImplementedException();
    public Task<object?> UpdateInventoryAsync(int sellerId, int productId, int quantity) => throw new NotImplementedException();
}
