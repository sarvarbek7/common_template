namespace Application.Common;

public class Pagination
{
    private const uint DefaultPageNumber = 1;
    private const uint DefaultPageSize = 25;
    
    private readonly uint pageNumber = DefaultPageNumber;
    private readonly uint pageSize = DefaultPageSize;

    private static Pagination @default = new Pagination(); 

    private Pagination()
    {
    }
    
    public Pagination(uint? pageNumber, uint? pageSize)
    {
        if (pageSize.HasValue && pageSize.Value != 0)
        {
            this.pageSize = pageSize.Value;
        }

        if (pageNumber.HasValue && pageNumber.Value != 0)
        {
            this.pageNumber = pageNumber.Value;
        }
    }
    
    public static Pagination Default => @default;
    
    public int Page => (int)pageNumber;
    public int Take => (int)pageSize;
    public int Skip => (Page - 1) * Take;
}