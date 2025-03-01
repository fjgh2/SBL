using SBL.Domain.Common;
using SBL.Domain.Entities;

namespace SBL.Services.Contracts.Repositories;

public interface IFeedbackRepository : IGenericRepository<Feedback>
{
    Task<Result<IEnumerable<Feedback>>> GetFeedbacksForProduct(int productId);
}
