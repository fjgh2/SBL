using SBL.Domain.Entities;

namespace SBL.Services.Contracts.Services;

public interface INewsService
{
    Task<IEnumerable<News>> GetNewssByConditionAsync(Func<News, bool> condition);

    Task<News> GetNewsByConditionAsync(Func<News, bool> condition);

    Task<int> CreateNewsAsync(News news);

    Task UpdateNewsAsync(News news);

    Task DeleteNewsAsync(int newsId);
}
