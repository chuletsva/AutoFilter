using System.Linq.Expressions;
using Autofilter.Model;

namespace Autofilter.Nodes;

class GroupNode : INode
{
    private readonly IEnumerable<INode> _children;

    public GroupNode(IEnumerable<INode> children, LogicOperator? logic)
    {
        (_children, Logic) = (children, logic);
    }

    public LogicOperator? Logic { get; }

    public Expression BuildExpression()
    {
        return _children.Skip(1).Aggregate(
            _children.First().BuildExpression(), 
            (current, node) => node.Logic switch
            {
                LogicOperator.And => Expression.AndAlso(current, node.BuildExpression()),
                LogicOperator.Or => Expression.OrElse(current, node.BuildExpression()),
                _ => throw new ArgumentOutOfRangeException(nameof(node.Logic))
            });
    }
}