using API.Entities.Products;

namespace API.Helpers.Products;

public static class ProductExtension
{
    public static IQueryable<Product> Search(this IQueryable<Product> query, string search)
    {
        if (string.IsNullOrWhiteSpace(search)) return query;

        var lowerCaseSearch = search.Trim().ToLower();
        return query.Where(p => p.Name.ToLower().Contains(lowerCaseSearch));
    }

    public static IQueryable<Product> Filter(this IQueryable<Product> query, string brands, string types)
    {
        var brandList = new List<string>();
        var typeList = new List<string>();

        if (!string.IsNullOrWhiteSpace(brands))
            brandList.AddRange(brands.ToLower().Split(',').ToList());

        if (!string.IsNullOrWhiteSpace(types))
            typeList.AddRange(types.ToLower().Split(',').ToList());

        query = query.Where(p => brandList.Count == 0 || brandList.Contains(p.Brand.Name.ToLower()));
        query = query.Where(p => typeList.Count == 0 || typeList.Contains(p.Type.Name.ToLower()));

        return query;
    }
}