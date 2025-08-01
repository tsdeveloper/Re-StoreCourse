using System.Linq.Expressions;
using System.Reflection;

namespace API.Helpers.Contexts;

public static class QueryableExtensions
{
    // O método de extensão continua genérico, o que é a forma correta.
    public static IQueryable<T> OrderByCustom<T>(this IQueryable<T> query, string field, string direction)
    {
        var type = typeof(T);
        
        var propInfo = type.GetProperty(field, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
        
        if (propInfo == null)
        {
            throw new ArgumentException($"The property '{field}' does not exist on type '{type.Name}'.");
        }

        var param = Expression.Parameter(type, "p");
        var property = Expression.Property(param, propInfo);
        var lambda = Expression.Lambda(property, param);
        
        var nameMethod = direction?.ToLower() == "desc" ? "OrderByDescending" : "OrderBy";

        var orderByMethod = typeof(Queryable).GetMethods()
            .Single(
                m => m.Name == nameMethod 
                     && m.IsGenericMethodDefinition 
                     && m.GetParameters().Length == 2
            )
            .MakeGenericMethod(typeof(T), propInfo.PropertyType);
        
        var result = orderByMethod.Invoke(null, new object[] { query, lambda });
        
        return (IQueryable<T>)result;
    }
}