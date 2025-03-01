using SBL.Domain.Entities;

namespace SBL.Services.Contracts.Services;

public interface IFeedbackService
{
    Task<Feedback> GetFeedbacksForProductAsync(int productId);

    Task LeaveFeedbackAsync(int productId, int userId, string text);
}
