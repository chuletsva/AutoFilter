using Autofilter.Model;

namespace Autofilter.Processors;

interface ISearchProcessor
{
    IQueryable<T> ApplySearch<T>(IQueryable<T> source, SearchRule[] search, GroupRule[]? groups);
}