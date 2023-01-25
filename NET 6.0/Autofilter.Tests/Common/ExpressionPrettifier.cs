using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Autofilter.Tests.Common;

internal static class ExpressionPrettifier
{
    public static string PrettifyLambda(LambdaExpression lambdaExpr)
    {
        string lambda = lambdaExpr.ToString();

        lambda = Regex.Replace(lambda, @"(?'prop'x.Prop)(?'num'\d+)", match => match.Groups["num"].Value);
        lambda = Regex.Replace(lambda, @"OrElse", _ => "or");
        lambda = Regex.Replace(lambda, @"AndAlso", _ => "and");
        lambda = Regex.Replace(lambda, @"x => ", _ => "");
        lambda = Regex.Replace(lambda, @"$((.+))^", _ => "");
        lambda = Regex.Replace(lambda, @"^\((?'content'.+)\)$", match => match.Groups["content"].Value);

        return lambda;
    }
}