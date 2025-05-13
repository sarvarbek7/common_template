using Microsoft.AspNetCore.Mvc;

namespace Contracts.Common;

public record PagedRecordQuery
{
    [FromQuery(Name = "page")]
    public uint? Page { get; init; }

    [FromQuery(Name = "pageSize")]
    public uint? PageSize { get; init; }
}

public class PagedClassQuery
{
    [FromQuery(Name = "page")]
    public uint? Page { get; init; }

    [FromQuery(Name = "pageSize")]
    public uint? PageSize { get; init; }
}