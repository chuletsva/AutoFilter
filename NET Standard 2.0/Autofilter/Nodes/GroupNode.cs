using System.Linq.Expressions;
using Autofilter.Model;

namespace Autofilter.Nodes;

class GroupNode : INode
{
    private readonly IReadOnlyList<INode> _children;

    public GroupNode(IReadOnlyList<INode> children, LogicOperator? logic)
    {
        (_children, Logic) = (children, logic);
    }

    public LogicOperator? Logic { get; }

    public Expression BuildExpression()
    {
        Expression result = _children[0].BuildExpression();

        for (int i = 1; i < _children.Count; i++)
        {
            switch (_children[i].Logic)
            {
                case LogicOperator.Or:
                    int j = i;

                    Expression andExpr = _children[i].BuildExpression();

                    while (j + 1 < _children.Count && _children[j + 1].Logic == LogicOperator.And)
                        andExpr = Expression.AndAlso(andExpr, _children[++j].BuildExpression());

                    result = Expression.OrElse(result, andExpr);

                    i = j;
                    break;
                case LogicOperator.And:
                    result = Expression.AndAlso(result, _children[i].BuildExpression());
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        return result;
    }
}