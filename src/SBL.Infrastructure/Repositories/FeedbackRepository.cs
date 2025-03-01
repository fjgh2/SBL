using Microsoft.EntityFrameworkCore;
using SBL.Domain.Common;
using SBL.Domain.Entities;
using SBL.Services.Contracts.Repositories;

namespace SBL.Infrastructure.Repositories;

public class FeedbackRepository : GenericRepository<Feedback>, IFeedbackRepository
{
    public FeedbackRepository(SblDbContext context) : base(context)
    {
    }

    public async Task<Result<IEnumerable<Feedback>>> GetFeedbacksForProduct(int productId)
    {
        try
        {
            var feedbacks = await _context.Feedbacks
                .Where(f => f.ProductId == productId)
                .AsNoTracking()
                .ToListAsync();

            return Result.Success((IEnumerable<Feedback>)feedbacks);
        }
        catch (Exception e)
        {
            return Result.Fail<IEnumerable<Feedback>>(
                $"Error fetching feedbacks for feedback {productId}: {e.Message}");
        }
    }
}