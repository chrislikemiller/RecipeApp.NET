namespace RecipeApp.API.Models;

public class PagedDataDTO<T>
{
    public IEnumerable<T> Data { get; set; } = Enumerable.Empty<T>();
    public int TotalCount { get; set; } = 0;
}
