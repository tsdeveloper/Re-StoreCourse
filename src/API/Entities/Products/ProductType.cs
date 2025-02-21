namespace API.Entities.Products;

public class ProductType : BaseEntity
{
  public string Name { get; set; }
  public ICollection<Product> ProductList { get; set; } = new List<Product>();
}
