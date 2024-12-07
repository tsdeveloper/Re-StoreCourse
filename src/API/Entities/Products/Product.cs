namespace API.Entities.Products;

public class Product : BaseEntity
{
  public int TypeId { get; set; }
  public int BrandId { get; set; }
  public string Name { get; set; }
  public string Description { get; set; }
  public long Price { get; set; }
  public string PictureUrl { get; set; }
  public ProductType Type { get; set; }
  public ProductBrand Brand { get; set; }
  public int QuantityInStock { get; set; }
}
