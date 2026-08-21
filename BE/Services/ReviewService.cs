namespace BE.Services;

public interface IReviewService
{
    Task<object?> GetReviewsAsync(object filter);
    Task<object?> ReplyReviewAsync(int sellerId, int reviewId, string response);
    Task<object?> GetFeedbackSummaryAsync(int sellerId);
}

public class ReviewService : IReviewService
{
    public Task<object?> GetReviewsAsync(object filter) => throw new NotImplementedException();
    public Task<object?> ReplyReviewAsync(int sellerId, int reviewId, string response) => throw new NotImplementedException();
    public Task<object?> GetFeedbackSummaryAsync(int sellerId) => throw new NotImplementedException();
}
