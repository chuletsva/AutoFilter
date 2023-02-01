using System.Linq.Expressions;
using Autofilter.Helpers;
using Autofilter.Nodes;
using Autofilter.Rules;

namespace Autofilter.Processors;

internal static class FilterProcessor
{
    public static IQueryable ApplyFilter(IQueryable queryable, FilterRule filter)
    {
        LambdaExpression lambdaExpr = BuildPredicate(queryable.ElementType, filter.Conditions, filter.Groups);

        var method = QueryableMethods.Where(queryable.ElementType);

        var filteredQueryable = method.Invoke(null, new object[] { queryable, lambdaExpr }) ?? throw new NullReferenceException();

        return (IQueryable) filteredQueryable;
    }

    public static LambdaExpression BuildPredicate(Type elementType, Condition[] conditions, Group[]? groups = default)
    {
        Group rootGroup = new
        (
            Start: 1,
            End: conditions.Length,
            Level: groups is { Length: > 0 } ? groups.Max(x => x.Level) + 1 : 0
        );

        ParameterExpression paramExpr = Expression.Parameter(elementType, "x");

        var levels = groups?.GroupBy(x => x.Level)
            .ToDictionary(x => x.Key, x => x.OrderBy(g => g.Start).ToArray());

        Expression bodyExpr = BuildGroupNode(rootGroup, conditions, levels, paramExpr).BuildExpression();

        var lambdaExpr = Expression.Lambda(bodyExpr, paramExpr);

        return lambdaExpr;
    }

    private static INode BuildGroupNode(
        Group parentGroup, Condition[] conditions,
        IReadOnlyDictionary<int, Group[]>? levels,
        ParameterExpression paramExpr)
    {
        List<Group> childGroups = new();

        if (levels is not null)
        {
            int currentLevel = parentGroup.Level - 1;

            while (currentLevel > 0 && childGroups.Count is 0)
            {
                if (levels.TryGetValue(currentLevel, out var groups))
                {
                    foreach (Group group in groups)
                    {
                        if (parentGroup.Start <= group.Start && group.End <= parentGroup.End)
                        {
                            childGroups.Add(group);
                        }
                    }
                }

                currentLevel--;
            }
        }

        List<INode> nodes = new();

        if (childGroups.Count > 0)
        {
            int j = 0;

            for (int i = parentGroup.Start - 1; i < parentGroup.End; i++)
            {
                if (j < childGroups.Count && childGroups[j].Start - 1 == i)
                {
                    Group group = childGroups[j++];
                    nodes.Add(BuildGroupNode(group, conditions, levels, paramExpr));
                    i = group.End - 1;
                }
                else nodes.Add(new SingleNode(conditions[i], paramExpr));
            }
        }
        else
        {
            for (int i = parentGroup.Start - 1; i < parentGroup.End; i++)
            {
                nodes.Add(new SingleNode(conditions[i], paramExpr));
            }
        }

        return new GroupNode(nodes, conditions[parentGroup.Start - 1].LogicOperator);
    }
}