using System.Linq.Expressions;
using Autofilter.Models;

namespace Autofilter.Nodes;

internal class GroupNode : INode
{
    private readonly IReadOnlyList<INode> _children;

    public GroupNode(IReadOnlyList<INode> children, LogicOperator? logicOperator)
    {
        (_children, Operator) = (children, logicOperator);
    }

    public LogicOperator? Operator { get; }

    public Expression BuildExpression()
    {
        Expression rightOperand = _children[0].BuildExpression();

        for (int i = 1; i < _children.Count; i++)
        {
            switch (_children[i].Operator)
            {
                case LogicOperator.Or:
                    int j = i;

                    Expression leftOperand = _children[i].BuildExpression();

                    while (j + 1 < _children.Count && _children[j + 1].Operator == LogicOperator.And)
                        leftOperand = Expression.AndAlso(leftOperand, _children[++j].BuildExpression());

                    rightOperand = Expression.OrElse(rightOperand, leftOperand);

                    i = j;
                    break;
                case LogicOperator.And:
                    rightOperand = Expression.AndAlso(rightOperand, _children[i].BuildExpression());
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        return rightOperand;
    }
}