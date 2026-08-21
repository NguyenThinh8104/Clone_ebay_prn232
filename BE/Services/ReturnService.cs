namespace BE.Services;

public interface IReturnService
{
    Task<object?> GetReturnRequestsAsync(int sellerId, object filter);
    Task<object?> UpdateReturnStatusAsync(int sellerId, int returnId, string status);
}

public class ReturnService : IReturnService
{
    public Task<object?> GetReturnRequestsAsync(int sellerId, object filter) => throw new NotImplementedException();
    public Task<object?> UpdateReturnStatusAsync(int sellerId, int returnId, string status) => throw new NotImplementedException();
}
