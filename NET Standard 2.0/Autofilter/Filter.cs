using Autofilter.Models;

namespace Autofilter;

public sealed record Filter(
    SearchRule[]? Search = default, 
    GroupRule[]? Groups = default,
    SortingRule[]? Sorting = default,
    PaginationRule? Pagination = default);