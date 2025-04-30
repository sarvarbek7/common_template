namespace Contracts.Common;


/// <summary>
/// General wrapper for list response
/// </summary>
/// <typeparam name="TData">Type of data</typeparam>
/// <param name="PageSize">Number of records per page</param>
/// <param name="Page">Page number user requested</param>
/// <param name="From">First record number returned to user</param>
/// <param name="To">Last record number returned to user</param>
/// <param name="TotalPages">Total count of pages</param>
/// <param name="TotalRecords">Total count of records</param>
/// <param name="List"></param>
public class ListResponse<TData>
{
    public static ListResponse<TData> EmptyListResponse => new();

    public int PageSize { get; init; }
    public int Page { get; init; }
    public int From { get; init; }
    public int To { get; init; }
    public int TotalPageCount { get; init; }
    public int TotalRecords { get; init; }
    public IReadOnlyList<TData> List { get; init; } = [];
}