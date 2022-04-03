using System.Linq.Expressions;
using Autofilter.Model;
using Autofilter.Nodes;

namespace Autofilter.Helpers;

static class PredicateBuilder
{
    public static Expression<Func<T, bool>> BuildPredicate<T>(
        SearchRule[] operands, GroupRule[]? groups = default)
    {
        GroupRule parentGroup = new
        (
            Start: 0,
            End: operands.Length - 1,
            Level: groups is { Length: > 0 } ? groups.Max(x => x.Level) + 1 : 0
        );

        ParameterExpression paramExpr = Expression.Parameter(typeof(T), "x");

        Expression operandsExpr = BuildGroupNode(parentGroup, operands, groups, paramExpr)
            .BuildExpression();

        return Expression.Lambda<Func<T, bool>>(operandsExpr, paramExpr);
    }

    private static INode BuildGroupNode(
        GroupRule parentGroup, SearchRule[] operands,
        GroupRule[]? groups, ParameterExpression paramExpr)
    {
        List<GroupRule> highLevelGroups = new();

        if (groups is { Length: > 0 })
        {
            bool[] isInGroup = new bool[operands.Length];

            var childGroups = groups
                .Where(x =>
                    x.Start >= parentGroup.Start &&
                    x.End <= parentGroup.End &&
                    x.Level < parentGroup.Level)
                .OrderByDescending(x => x.Level)
                .ToArray();

            foreach (GroupRule group in childGroups)
            {
                if (isInGroup[group.Start]) continue;
                highLevelGroups.Add(group);
                Array.Fill(isInGroup, true, group.Start, group.End - group.Start + 1);
            }

            highLevelGroups.Sort((x, y) => x.Start.CompareTo(y.Start));
        }

        List<INode> nodes = new();

        int j = 0;

        for (int i = parentGroup.Start; i <= parentGroup.End; i++)
        {
            if (j < highLevelGroups.Count && highLevelGroups[j].Start == i)
            {
                GroupRule group = highLevelGroups[j++];
                nodes.Add(BuildGroupNode(group, operands, groups, paramExpr));
                i = group.End;
            }
            else nodes.Add(new SingleNode(operands[i], paramExpr));
        }

        return new GroupNode(nodes, operands[parentGroup.Start].LogicOperator);
    }
}