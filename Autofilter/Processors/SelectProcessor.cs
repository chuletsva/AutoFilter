using System.Linq.Expressions;
using System.Reflection;
using Autofilter.Helpers;

namespace Autofilter.Processors;

internal static class SelectProcessor
{
    public static IQueryable ApplySelect(IQueryable queryable, string[] propertyNames)
    {
        var properties = new PropertyInfo[propertyNames.Length];

        for (int i = 0; i < propertyNames.Length; i++)
        {
            properties[i] = ReflectionHelper.GetProperty(queryable.ElementType, propertyNames[i]);
        }

        ParameterExpression paramExpr = Expression.Parameter(queryable.ElementType, "x");

        Type destinationType = typeof(Dictionary<string, object>);

        var addMethod = destinationType.GetMethod("Add") ?? throw new NullReferenceException();

        ListInitExpression bodyExpr = Expression.ListInit(
            Expression.New(destinationType),
            properties.Select(x => Expression.ElementInit(
                addMethod,
                Expression.Constant(x.Name),
                Expression.Convert(Expression.Property(paramExpr, x), typeof(object)))));

        MethodInfo method = LinqMethods.Select(queryable.ElementType, destinationType);

        LambdaExpression lambdaExpr = Expression.Lambda(bodyExpr, paramExpr);

        var selectQueryable = method.Invoke(null, new object?[] { queryable, lambdaExpr }) ?? throw new NullReferenceException();

        return (IQueryable)selectQueryable;
    }
}