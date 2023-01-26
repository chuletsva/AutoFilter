using System.Linq.Expressions;
using System.Reflection;
using Autofilter.Helpers;

namespace Autofilter.Processors;

internal static class SelectProcessor
{
    public static IQueryable ApplySelectDictionary(IQueryable queryable, string[] propertyNames)
    {
        var properties = ReflectionHelper.GetProperties(queryable.ElementType, propertyNames).ToArray();

        ParameterExpression paramExpr = Expression.Parameter(queryable.ElementType, "x");

        Type dictType = typeof(Dictionary<string, object>);

        var addMethod = dictType.GetMethod("Add") ?? throw new NullReferenceException();

        ListInitExpression bodyExpr = Expression.ListInit(
            Expression.New(dictType),
            properties.Select(x => Expression.ElementInit(
                addMethod,
                Expression.Constant(x.Name),
                Expression.Convert(Expression.Property(paramExpr, x), typeof(object)))));

        MethodInfo method = QueryableMethods.Select(queryable.ElementType, dictType);

        LambdaExpression lambdaExpr = Expression.Lambda(bodyExpr, paramExpr);

        var selectQueryable = method.Invoke(null, new object?[] { queryable, lambdaExpr }) ?? throw new NullReferenceException();

        return (IQueryable) selectQueryable;
    }
}