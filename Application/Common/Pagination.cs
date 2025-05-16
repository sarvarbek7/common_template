namespace Application.Common;

public class Pagination
{
#pragma warning disable CA2211 // Non-constant fields should not be visible
    public static uint DefaultPageSize = 25;
    public static uint MaxPageSize = 500;
#pragma warning restore CA2211 // Non-constant fields should not be visible

    private const uint DefaultPageNumber = 1;
    
    private readonly uint pageNumber = DefaultPageNumber;
    private readonly uint pageSize = DefaultPageSize;

    private static readonly Pagination @default = new(); 

    private Pagination()
    {
    }
    
    public Pagination(uint? pageNumber, uint? pageSize)
    {
        if (pageSize.HasValue && pageSize.Value != 0)
        {
            if (pageSize <= MaxPageSize)
            {
                this.pageSize = pageSize.Value;
            }
            else
            {
                this.pageSize = MaxPageSize;
            }
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