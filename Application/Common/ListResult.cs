namespace Application.Common;

public class ListResult<TData>
{
    public PageDetail PageDetail { get; init; }
    public IQueryable<TData>? Query { get; init; } = null;
    public ICollection<TData>? Data { get; init; } = null;

    public bool IsQueryable => Query != null;

    private ListResult(Pagination pagination, int recordsCount)
    {
        PageDetail = new PageDetail(pagination, recordsCount);
    }

    internal static ListResult<TData> FromQueryable(IQueryable<TData> query,
                                              Pagination pagination,
                                              int recordsCount)
    {
        return new ListResult<TData>(pagination, recordsCount)
        {
            Query = query
        };
    }

    internal static ListResult<TData> FromCollection(ICollection<TData> data,
                                              Pagination pagination,
                                              int recordsCount)
    {
        return new ListResult<TData>(pagination, recordsCount)
        {
            Data = data
        };
    }
}