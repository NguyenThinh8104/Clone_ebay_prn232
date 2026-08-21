namespace BE.Services;

public interface IAuthService
{
    Task<object?> RegisterAsync(object request);
    Task<object?> LoginAsync(object request);
    Task<object?> RefreshTokenAsync(object request);
    Task RevokeTokenAsync(object request);
}

public class AuthService : IAuthService
{
    public Task<object?> RegisterAsync(object request) => throw new NotImplementedException();
    public Task<object?> LoginAsync(object request) => throw new NotImplementedException();
    public Task<object?> RefreshTokenAsync(object request) => throw new NotImplementedException();
    public Task RevokeTokenAsync(object request) => throw new NotImplementedException();
}
