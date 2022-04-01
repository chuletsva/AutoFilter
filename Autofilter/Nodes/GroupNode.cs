using System.Linq.Expressions;
using Autofilter.Model;

namespace Autofilter.Nodes;

class GroupNode : INode
{
    private readonly IEnumerable<INode> _children;

    public GroupNode(IEnumerable<INode> children, ComposeOperator? compose)
    {
        (_children, Compose) = (children, compose);
    }

    public ComposeOperator? Compose { get; }

    public Expression BuildExpression()
    {
        return _children.Skip(1).Aggregate(
            seed: _children.First().BuildExpression(),
            (Expression expr, INode node) => node.Compose switch
            {
                ComposeOperator.And => Expression.AndAlso(expr, node.BuildExpression()),
                ComposeOperator.Or => Expression.OrElse(expr, node.BuildExpression()),
                _ => throw new ArgumentOutOfRangeException(nameof(node.Compose))
            });
    }
}