using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeApp.Application.Models;
public record struct RecipeQueryOptions
{
    public RecipeQueryOptions()
    {
    }

    public Guid UserId { get; set; }
    public int CurrentPage { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SearchTerm { get; set; } 
    public int? Difficulty { get; set; }
    public RecipeSortType SortType { get; set; } = RecipeSortType.Date;
    public SortDirection SortDirection { get; set; } = SortDirection.Descending;
}

