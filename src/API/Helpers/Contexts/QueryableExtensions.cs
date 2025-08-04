using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;

namespace API.Helpers.Contexts;

public static class QueryableExtensions
{
    // O método de extensão continua genérico, o que é a forma correta.
    public static IQueryable<T> OrderByCustom<T>(this IQueryable<T> query, string field, string direction)
    {
        if (string.IsNullOrWhiteSpace(field))
            return query;
        
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
    
    public static decimal ConvertToBrl(this decimal dollarAmount, decimal exchangeRate)
    {
        if (dollarAmount < 0)
        {
            throw new ArgumentException("O valor em dólar não pode ser negativo.", nameof(dollarAmount));
        }

        if (exchangeRate <= 0)
        {
            throw new ArgumentException("A taxa de câmbio deve ser um valor positivo.", nameof(exchangeRate));
        }

        return dollarAmount * exchangeRate;
    }
    
    public static string ConvertToBrlFormatted(this decimal dollarAmount, decimal exchangeRate)
    {
        var brlValue = dollarAmount.ConvertToBrl(exchangeRate);

        // Usa a cultura brasileira para formatar o valor com o símbolo "R$" e duas casas decimais
        var cultureInfo = new CultureInfo("pt-BR");
        return brlValue.ToString("C2", cultureInfo);
    }
}