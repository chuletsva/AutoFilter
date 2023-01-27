using System.Linq.Expressions;
using Autofilter.Nodes;
using Autofilter.Rules;

namespace Autofilter.Processors;

internal static class FilterProcessor
{
    public static IQueryable ApplyFilter(IQueryable queryable, FilterRule filter)
    {
        if (filter.Conditions.Length is 0) return queryable;

        LambdaExpression lambda = BuildPredicate(queryable.ElementType, filter.Conditions, filter.Groups);

        var method = typeof(Queryable).GetMethods()
            .First(x => x.Name == "Where" && x.GetParameters().Length == 2)
            .MakeGenericMethod(queryable.ElementType);

        var filteredQueryable = method.Invoke(null, new object[] { queryable, lambda }) ?? throw new NullReferenceException();

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

        Dictionary<int, Group[]>? groupsByLevel = groups?.GroupBy(x => x.Level)
            .ToDictionary(x => x.Key, x => x.OrderBy(g => g.Start).ToArray());

        Expression bodyExpr = BuildGroupNode(rootGroup, conditions, groupsByLevel, paramExpr).BuildExpression();

        var lambdaExpr = Expression.Lambda(bodyExpr, paramExpr);

        return lambdaExpr;
    }

    private static INode BuildGroupNode(
        Group parentGroup, Condition[] conditions,
        IReadOnlyDictionary<int, Group[]>? groupsByLevel,
        ParameterExpression paramExpr)
    {
        List<Group> childGroups = new();

        if (groupsByLevel is not null)
        {
            int currentLevel = parentGroup.Level - 1;

            while (currentLevel > 0 && childGroups.Count is 0)
            {
                if (groupsByLevel.TryGetValue(currentLevel, out var currentLevelGroups))
                {
                    foreach (Group group in currentLevelGroups)
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
                    nodes.Add(BuildGroupNode(group, conditions, groupsByLevel, paramExpr));
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