namespace TaskManagement.Application.DTOs.QueryParameters;

public class CategoryQueryParametersDto
{

    private const int MaxPageSize = 50;
    private const int DefaultPageSize = 10;
    private const int DefaultPageNumber = 1;
    private int _pageNumber = DefaultPageNumber;
    public int PageNumber
    {
        get => _pageNumber;
        set => _pageNumber = value <= 0 ? DefaultPageNumber : value;
    }
    private int _pageSize = DefaultPageSize;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value <= 0 ? DefaultPageSize : Math.Min(value, MaxPageSize);
    }



    //Search
    public string? Search { get; set; }// for name or description

    //Sorting
    public string? SortBy { get; set; } = "createdAt";
    public string? SortDirection { get; set; } = "desc";


}
