using System.Linq.Expressions;
using Autofilter.Rules;

namespace Autofilter.Nodes;

internal interface INode
{
    LogicOperator? Operator { get; }
    Expression BuildExpression();
}