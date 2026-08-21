namespace BE.Services;

public interface ICouponService
{
    Task<object?> CreateCouponAsync(int sellerId, object request);
    Task<object?> GetCouponsAsync(int sellerId, object filter);
    Task<object?> UpdateCouponAsync(int sellerId, int id, object request);
    Task DeleteCouponAsync(int sellerId, int id);
    Task<object?> ValidateCouponAsync(string code, int productId);
    Task<object?> GetUsageStatsAsync(int sellerId, int id);
}

public class CouponService : ICouponService
{
    public Task<object?> CreateCouponAsync(int sellerId, object request) => throw new NotImplementedException();
    public Task<object?> GetCouponsAsync(int sellerId, object filter) => throw new NotImplementedException();
    public Task<object?> UpdateCouponAsync(int sellerId, int id, object request) => throw new NotImplementedException();
    public Task DeleteCouponAsync(int sellerId, int id) => throw new NotImplementedException();
    public Task<object?> ValidateCouponAsync(string code, int productId) => throw new NotImplementedException();
    public Task<object?> GetUsageStatsAsync(int sellerId, int id) => throw new NotImplementedException();
}
