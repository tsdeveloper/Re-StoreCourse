namespace API.RequestsHelpers.Products;

public class ProductParams : PaginationParams
{
    public  string? OrderBy { get; set; }
    public  string? Direction { get; set; }
    public string? SearchTherm { get; set; }
    public string? Brands { get; set; }
    public string? Types { get; set; }
}