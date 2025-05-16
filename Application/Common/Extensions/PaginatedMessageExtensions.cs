namespace Application.Common.Extensions;

public static class PaginatedMessageExtensions
{
    public static ListResult<T> ToListResultWithQuery<T>(this IPaginatedMessage message,
                                                                 IQueryable<T> query,
                                                                 int recordsCount)
     => ListResult<T>.FromQueryable(query,
                                        message.Pagination,
                                        recordsCount);

    public static ListResult<T> ToListResultWithData<T>(this IPaginatedMessage message,
                                                        ICollection<T> data,
                                                        int recordsCount)
    => ListResult<T>.FromCollection(data,
                                    message.Pagination,
                                    recordsCount);
}