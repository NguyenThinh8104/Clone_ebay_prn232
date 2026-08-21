namespace BE.Services;

public interface IProductService
{
    Task<object?> CreateProductAsync(int sellerId, object request);
    Task<object?> GetProductsAsync(object filter);
    Task<object?> GetProductByIdAsync(int id);
    Task<object?> UpdateProductAsync(int sellerId, int id, object request);
    Task HideProductAsync(int sellerId, int id);
    Task DeleteProductAsync(int sellerId, int id);
    Task<object?> GetProductBidsAsync(int sellerId, int productId);
    Task<object?> CloseAuctionAsync(int productId);
}

public class ProductService : IProductService
{
    public Task<object?> CreateProductAsync(int sellerId, object request) => throw new NotImplementedException();
    public Task<object?> GetProductsAsync(object filter) => throw new NotImplementedException();
    public Task<object?> GetProductByIdAsync(int id) => throw new NotImplementedException();
    public Task<object?> UpdateProductAsync(int sellerId, int id, object request) => throw new NotImplementedException();
    public Task HideProductAsync(int sellerId, int id) => throw new NotImplementedException();
    public Task DeleteProductAsync(int sellerId, int id) => throw new NotImplementedException();
    public Task<object?> GetProductBidsAsync(int sellerId, int productId) => throw new NotImplementedException();
    public Task<object?> CloseAuctionAsync(int productId) => throw new NotImplementedException();
}
