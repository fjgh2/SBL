using SBL.Domain.Entities;
using SBL.Services.Contracts.Services;

namespace SBL.Services.Ordering;

public class FeedbackService : IFeedbackService
{
    public async Task<Feedback> GetFeedbacksForProductAsync(int productId)
    {
        throw new NotImplementedException();
    }

    public async Task LeaveFeedbackAsync(int productId, int userId, string text)
    {
        throw new NotImplementedException();
    }
}
