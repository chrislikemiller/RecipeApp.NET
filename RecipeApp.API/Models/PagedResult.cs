using RecipeApp.Application.DTOs;
using RecipeApp.Application.Models;

namespace RecipeApp.API.Models;

public class PagedResult<T>
{
    public IEnumerable<T> Data { get; set; } = [];
    public PaginationMetadata Metadata { get; set; } = new();
    public PaginationLinks Links { get; set; } = new();

    public PagedResult(string sourceEndpoint, PagedDataDTO<T> pagedData, RecipeQueryOptions queryOptions)
    {
        var totalPages = (int)Math.Ceiling((double)pagedData.TotalCount / queryOptions.PageSize);

        var metadata = new PaginationMetadata
        {
            CurrentPage = queryOptions.CurrentPage,
            PageSize = queryOptions.PageSize,
            TotalCount = pagedData.TotalCount,
            TotalPages = Math.Max(1, totalPages),
            HasPreviousPage = queryOptions.CurrentPage > 1,
            HasNextPage = queryOptions.CurrentPage < totalPages
        };

        var links = new PaginationLinks
        {
            Self = new PaginationLink
            {
                BaseUrl = sourceEndpoint,
                QueryOptions = queryOptions
            },
            First = new PaginationLink
            {
                BaseUrl = sourceEndpoint,
                QueryOptions = queryOptions with { CurrentPage = 1 },
            },
            Last = new PaginationLink
            {
                BaseUrl = sourceEndpoint,
                QueryOptions = queryOptions with { CurrentPage = totalPages },
            },
        };

        if (metadata.HasNextPage)
        {
            links.Next = new PaginationLink
            {
                BaseUrl = sourceEndpoint,
                QueryOptions = queryOptions with
                {
                    CurrentPage = queryOptions.CurrentPage + 1
                },
            };
        }

        if (metadata.HasPreviousPage)
        {
            links.Previous = new PaginationLink
            {
                BaseUrl = sourceEndpoint,
                QueryOptions = queryOptions with
                {
                    CurrentPage = queryOptions.CurrentPage - 1
                },
            };
        }

        Data = pagedData.Data;
        Metadata = metadata;
        Links = links;
    }
}

public class PaginationMetadata
{
    private int _minPageSize = 5;
    private int _maxPageSize = 50;
    private int _pageSize = 10;

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > _maxPageSize
            ? _maxPageSize
            : Math.Max(_minPageSize, value);
    }
    public int CurrentPage { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
    public bool HasNextPage { get; set; }
    public bool HasPreviousPage { get; set; }
}

public class PaginationLink
{
    public string BaseUrl { get; set; } = string.Empty;
    public RecipeQueryOptions QueryOptions { get; set; } = new RecipeQueryOptions();
}

public class PaginationLinks
{
    public PaginationLink? Previous { get; set; }
    public PaginationLink? Next { get; set; }
    public PaginationLink Self { get; set; } = default!;
    public PaginationLink First { get; set; } = default!;
    public PaginationLink Last { get; set; } = default!;
}