using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.DTOs;

public class TodoQueryParametersDto
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
    //Filtering
    public TodoItemStatus? Status { get; set; }
    public DateTime? DueBefore { get; set; }
    public DateTime? DueAfter { get; set; }
    //Search
    public string? Search { get; set; }// for title or description
    //Sorting
    public string? SortBy { get; set; } = "createdAt";
    public string? SortDirection { get; set; } = "desc";


}
