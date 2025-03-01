using System.Linq.Expressions;

namespace SBL.Infrastructure;

public static class IQueryableExtensions
{
    public static IQueryable<T> FilterByTags<T>(this IQueryable<T> queryable, 
        Expression<Func<T, IEnumerable<int>>> tagIdsSelector, 
        IEnumerable<int> tagIds)
    {
        if (!tagIds.Any())
        {
            return queryable;
        }
        
        return queryable.Where(entity => 
            tagIds.Any(tagId => 
            tagIdsSelector.Compile().Invoke(entity)
                .Contains(tagId)));
    }
    
    public static IQueryable<T> FilterByTagNames<T>(this IQueryable<T> queryable, 
        Expression<Func<T, IEnumerable<string>>> tagNamesSelector, 
        IEnumerable<string> tagNames)
    {
        if (!tagNames.Any())
        {
            return queryable;
        }
            
        var normalizedTagNames = tagNames.Select(t => t.ToLower()).ToList();
        
        return queryable.Where(entity => 
            tagNamesSelector.Compile().Invoke(entity)
                .Select(t => t.ToLower())
                .Any(tagName => normalizedTagNames.Contains(tagName)));
    }
}
