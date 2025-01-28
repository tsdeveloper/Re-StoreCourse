namespace API.Entities.Products;

public class ProductBrand : BaseEntity
{
  public string Name { get; set; }
  public ICollection<Product> ProductList { get; set; } = new List<Product>();
}
