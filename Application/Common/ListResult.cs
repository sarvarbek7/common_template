namespace Application.Common;

public class ListResult<TData>(IQueryable<TData> query, Pagination pagination, int recordsCount)
{
    public PageDetail PageDetail { get; init; } = new PageDetail(pagination, recordsCount);
    public IQueryable<TData> Query { get; init; } = query;
}