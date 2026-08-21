namespace BE.Services;

public interface IDashboardService
{
    Task<object?> GetSummaryAsync(int sellerId, string period);
    Task<object?> GetTopProductsAsync(int sellerId, string period, int limit);
    Task<object?> GetRevenueChartAsync(int sellerId, string period);
    Task<object?> GetPerformanceAsync(int sellerId, string period);
}

public class DashboardService : IDashboardService
{
    public Task<object?> GetSummaryAsync(int sellerId, string period) => throw new NotImplementedException();
    public Task<object?> GetTopProductsAsync(int sellerId, string period, int limit) => throw new NotImplementedException();
    public Task<object?> GetRevenueChartAsync(int sellerId, string period) => throw new NotImplementedException();
    public Task<object?> GetPerformanceAsync(int sellerId, string period) => throw new NotImplementedException();
}
