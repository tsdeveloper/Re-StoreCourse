using API.Entities.Products;

namespace API.Entities.Baskets;

public class BasketItem : BaseEntity    
{
    public int Quantity { get; set; }
    public int ProductId { get; set; }
    public int BasketId { get; set; }
    public Product Product { get; set; }
    public Basket Basket { get; set; }
}