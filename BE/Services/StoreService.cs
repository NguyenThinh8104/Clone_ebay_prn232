namespace BE.Services;

public interface IStoreService
{
    Task<object?> ApplySellerAsync(int userId, object request);
    Task<object?> GetVerificationStatusAsync(int userId);
    Task<object?> ApproveSellerAsync(int storeId);
    Task<object?> RejectSellerAsync(int storeId, string reason);
    Task<object?> GetPublicStoreProfileAsync(int sellerId);
    Task<object?> UpdateStoreProfileAsync(int sellerId, object request);
}

public class StoreService : IStoreService
{
    public Task<object?> ApplySellerAsync(int userId, object request) => throw new NotImplementedException();
    public Task<object?> GetVerificationStatusAsync(int userId) => throw new NotImplementedException();
    public Task<object?> ApproveSellerAsync(int storeId) => throw new NotImplementedException();
    public Task<object?> RejectSellerAsync(int storeId, string reason) => throw new NotImplementedException();
    public Task<object?> GetPublicStoreProfileAsync(int sellerId) => throw new NotImplementedException();
    public Task<object?> UpdateStoreProfileAsync(int sellerId, object request) => throw new NotImplementedException();
}
