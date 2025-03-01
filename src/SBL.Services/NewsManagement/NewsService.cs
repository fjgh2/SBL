using SBL.Domain.Entities;
using SBL.Services.Contracts.Services;

namespace SBL.Services.NewsManagement;

public class NewsService : INewsService
{
    public Task<IEnumerable<News>> GetNewssByConditionAsync(Func<News, bool> condition)
    {
        throw new NotImplementedException();
    }

    public Task<News> GetNewsByConditionAsync(Func<News, bool> condition)
    {
        throw new NotImplementedException();
    }

    public Task<int> CreateNewsAsync(News news)
    {
        throw new NotImplementedException();
    }

    public Task UpdateNewsAsync(News news)
    {
        throw new NotImplementedException();
    }

    public Task DeleteNewsAsync(int newsId)
    {
        throw new NotImplementedException();
    }
}
