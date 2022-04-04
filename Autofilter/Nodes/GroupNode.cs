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
            if (_children[i].Logic == LogicOperator.Or)
            {
                int j = i;

                Expression andExpr = _children[i].BuildExpression();

                while (j + 1 < _children.Count && _children[j + 1].Logic == LogicOperator.And)
                    andExpr = Expression.AndAlso(andExpr, _children[++j].BuildExpression());

                result = Expression.OrElse(result, andExpr);

                i = j;
            }
            else if (_children[i].Logic == LogicOperator.And)
                result = Expression.AndAlso(result, _children[i].BuildExpression());
            else
                throw new Exception();
        }

        return result;

        //return _children.Skip(1).Aggregate(_children[0].BuildExpression(), (cur, node) => node.Logic switch
        //{
        //    LogicOperator.And => Expression.And(cur, node.BuildExpression()),
        //    LogicOperator.Or => Expression.Or(cur, node.BuildExpression()),
        //    _ => throw new Exception()
        //});
    }
}