namespace BE.Services;

public interface IDisputeService
{
    Task<object?> GetDisputesAsync(int sellerId, object filter);
    Task<object?> ResolveDisputeAsync(int userId, int disputeId, string resolution);
}

public class DisputeService : IDisputeService
{
    public Task<object?> GetDisputesAsync(int sellerId, object filter) => throw new NotImplementedException();
    public Task<object?> ResolveDisputeAsync(int userId, int disputeId, string resolution) => throw new NotImplementedException();
}
