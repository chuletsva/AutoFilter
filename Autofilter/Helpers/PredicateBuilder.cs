using System.Linq.Expressions;
using Autofilter.Models;
using Autofilter.Nodes;

namespace Autofilter.Helpers;

internal static class PredicateBuilder
{
    public static Expression<Func<T, bool>> Build<T>(SearchRule[] operands, GroupRule[]? groups = default)
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
        List<GroupRule> childGroups = new();

        if (groupsByLevel is not null)
        {
            int currentLevel = parentGroup.Level - 1;

            while (currentLevel > 0 && childGroups.Count is 0)
            {
                if (groupsByLevel.TryGetValue(currentLevel, out var currentLevelGroups))
                {
                    foreach (GroupRule group in currentLevelGroups)
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
                    GroupRule group = childGroups[j++];
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