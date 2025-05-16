namespace Application.Common;

public static class QueryableExtensions
{
    public static IQueryable<T> Paged<T>(this IQueryable<T> query, Pagination pagination)
    {
        return query.Skip(pagination.Skip)
                    .Take(pagination.Take);
    }
}