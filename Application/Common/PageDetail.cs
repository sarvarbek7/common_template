namespace Application.Common;

public class PageDetail
{
    private static PageDetail @default = new PageDetail();
    
    private PageDetail()
    {
    }

    public PageDetail(Pagination pagination, int totalRecords)
    {
        TotalRecords = totalRecords;
        Pagination = pagination;

        if (totalRecords < 0)
        {
            totalRecords = 0;
        }

        TotalPageCount = (int)Math.Ceiling((double)totalRecords / pagination.Take);

        From = pagination.Skip + 1;

        To = pagination.Skip + pagination.Take;

        if (To > totalRecords)
        {
            To = totalRecords;
        }

        if (From > totalRecords)
        {
            From = 0;
            To = 0;
        }
    }

    public static PageDetail Default => @default;

    public int From { get; private set; }
    public int To { get; private set; }
    public int TotalPageCount { get; private set; }
    public int TotalRecords { get; private set; }
    public Pagination Pagination { get; init; } = Pagination.Default;
}