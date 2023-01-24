using System.Linq.Expressions;
using Autofilter.Models;
using Autofilter.Nodes;

namespace Autofilter.Helpers;

internal static class PredicateBuilder
{
    public static Expression<Func<T, bool>> BuildPredicate<T>(
        SearchRule[] operands, GroupRule[]? groups = default)
    {
        GroupRule rootGroup = new
        (
            Start: 1,
            End: operands.Length,
            Level: groups is { Length: > 0 } ? groups.Max(x => x.Level) + 1 : 0
        );

        ParameterExpression paramExpr = Expression.Parameter(typeof(T), "x");

        Dictionary<int, GroupRule[]>? groupsByLevel = groups?.GroupBy(x => x.Level)
            .ToDictionary(x => x.Key, x => x.OrderBy(g => g.Start).ToArray());

        Expression bodyExpr = BuildGroupNode(rootGroup, operands, groupsByLevel, paramExpr).BuildExpression();

        var lambdaExpr = Expression.Lambda<Func<T, bool>>(bodyExpr, paramExpr);

        return lambdaExpr;
    }

    private static INode BuildGroupNode(
        GroupRule parentGroup, SearchRule[] operands,
        IReadOnlyDictionary<int, GroupRule[]>? groupsByLevel, 
        ParameterExpression paramExpr)
    {
        GroupRule[]? currentLevelGroups = default;

        if (groupsByLevel is not null)
        {
            int currentLevel = parentGroup.Level - 1;

            while (currentLevel > 0 && !groupsByLevel.TryGetValue(currentLevel, out currentLevelGroups))
            {
                currentLevel--;
            }
        }

        List<INode> nodes = new();

        if (currentLevelGroups is { Length: > 0 })
        {
            int j = 0;

            for (int i = parentGroup.Start - 1; i < parentGroup.End; i++)
            {
                if (j < currentLevelGroups.Length && currentLevelGroups[j].Start - 1 == i)
                {
                    GroupRule group = currentLevelGroups[j++];
                    nodes.Add(BuildGroupNode(group, operands, groupsByLevel, paramExpr));
                    i = group.End - 1;
                }
                else nodes.Add(new SingleNode(operands[i], paramExpr));
            }
        }
        else
        {
            for (int i = parentGroup.Start - 1; i < parentGroup.End; i++)
            {
                nodes.Add(new SingleNode(operands[i], paramExpr));
            }
        }

        return new GroupNode(nodes, operands[parentGroup.Start - 1].LogicOperator);
    }
}