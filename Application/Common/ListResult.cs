using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Application.Common;

public class ListResult<TData>
{
    private readonly IQueryable<TData>? query = null;
    private readonly ICollection<TData>? data = null;

    public PageDetail PageDetail { get; init; }
    public IQueryable<TData>? Query => query;
    public ICollection<TData>? Data => data;

    public bool IsQueryable => Query != null;

    private ListResult(Pagination pagination, int recordsCount)
    {
        PageDetail = new PageDetail(pagination, recordsCount);
    }

    private ListResult(Pagination pagination,
                       int recordsCount,
                       IQueryable<TData> query) : this(pagination,
                                                       recordsCount)
    {
        this.query = query;
    }

    private ListResult(Pagination pagination,
                       int recordsCount,
                       ICollection<TData> data) : this(pagination,
                                                       recordsCount)
    {
        this.data = data;
    }

    internal static ListResult<TData> FromQueryable(IQueryable<TData> query,
                                              Pagination pagination,
                                              int recordsCount)
    {
        return new ListResult<TData>(pagination, recordsCount, query);
    }

    internal static ListResult<TData> FromCollection(ICollection<TData> data,
                                              Pagination pagination,
                                              int recordsCount)
    {
        return new ListResult<TData>(pagination, recordsCount, data);
    }

    public async Task<List<TProjection>> ProjectTo<TProjection>(Expression<Func<TData, TProjection>> selector,
                                                                CancellationToken cancellationToken = default)
    {
        if (IsQueryable)
        {
            return await Query!.Select(selector).ToListAsync(cancellationToken);
        }
        else
        {
            return [.. Data!.Select(selector.Compile())];
        }

    }

    public async Task<IEnumerable<TData>> GetData(CancellationToken cancellationToken = default)
    {
        if (IsQueryable)
        {
            return await Query!.ToListAsync(cancellationToken);
        }
        else
        {
            return [.. Data!];
        }
    }
}