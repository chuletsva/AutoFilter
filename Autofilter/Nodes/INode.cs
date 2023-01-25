using System.Linq.Expressions;
using Autofilter.Models;

namespace Autofilter.Nodes;

internal interface INode
{
    LogicOperator? Operator { get; }
    Expression BuildExpression();
}