using System.Linq.Expressions;
using Autofilter.Model;

namespace Autofilter.Nodes;

interface INode
{
    ComposeOperator? Compose { get; }
    Expression BuildExpression();
}