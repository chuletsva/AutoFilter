using System.Linq.Expressions;
using Autofilter.Model;

namespace Autofilter.Nodes;

interface INode
{
    LogicOperator? Logic { get; }
    Expression BuildExpression();
}