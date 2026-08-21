namespace BE.Services;

public interface IMessageService
{
    Task<object?> GetConversationsAsync(int userId);
    Task<object?> GetMessageHistoryAsync(int userId, int withUserId, int page, int pageSize);
    Task<object?> SendMessageAsync(int senderId, int receiverId, string content);
    Task MarkAsReadAsync(int userId, int messageId);
}

public class MessageService : IMessageService
{
    public Task<object?> GetConversationsAsync(int userId) => throw new NotImplementedException();
    public Task<object?> GetMessageHistoryAsync(int userId, int withUserId, int page, int pageSize) => throw new NotImplementedException();
    public Task<object?> SendMessageAsync(int senderId, int receiverId, string content) => throw new NotImplementedException();
    public Task MarkAsReadAsync(int userId, int messageId) => throw new NotImplementedException();
}
