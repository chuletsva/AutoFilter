using System.Linq.Expressions;
using System.Reflection;
using Autofilter.Helpers;

namespace Autofilter.Processors;

internal static class SelectProcessor
{
    public static object ApplySelect(IQueryable queryable, string[] propertyNames)
    {
        var properties = new PropertyInfo[propertyNames.Length];

        for (int i = 0; i < propertyNames.Length; i++)
        {
            properties[i] = Reflection.GetProperty(queryable.ElementType, propertyNames[i]);
        }

        ParameterExpression paramExpr = Expression.Parameter(queryable.ElementType, "x");

        Type destinationType = typeof(Dictionary<string, object>);

        var addMethod = destinationType.GetMethod("Add") ?? throw new NullReferenceException();

        ListInitExpression body = Expression.ListInit(
            Expression.New(destinationType), 
            properties.Select(x => Expression.ElementInit(
                addMethod,
                Expression.Constant(x.Name),
                Expression.Convert(Expression.Property(paramExpr, x), typeof(object)))));

        MethodInfo method = typeof(Queryable).GetMethods()
            .First(x => x.Name == "Select" && x.GetParameters().Length == 2)
            .MakeGenericMethod(queryable.ElementType, destinationType);

        var lambda = Expression.Lambda(body, paramExpr);

        var selectQueryable = method.Invoke(null, new object?[] { queryable, lambda }) ?? throw new NullReferenceException();

        return (IQueryable)selectQueryable;
    }
}