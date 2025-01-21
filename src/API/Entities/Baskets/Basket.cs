using API.Entities.Products;

namespace API.Entities.Baskets;

public class Basket : BaseEntity
{
    public string BuyerId { get; set; }
    public ICollection<BasketItem> BasketItems { get; set; }

    public void AddItem(Product product, int quantity)
    {
        if (BasketItems.All(x => x.ProductId != product.Id))
        {
            BasketItems.Add(new BasketItem { ProductId = product.Id, Quantity = quantity });
        }
        
        var existingItem = BasketItems.FirstOrDefault(x => x.ProductId == product.Id);
        if (existingItem != null) existingItem.Quantity += quantity;
    }

    public void RemoveItem(Product product, int quantity)
    {
        var existingItem = BasketItems.FirstOrDefault(x => x.ProductId == product.Id);
        if (existingItem == null) return;
        
        existingItem.Quantity -= quantity;
        
        if (existingItem.Quantity == 0) BasketItems.Remove(existingItem);
    }
    
}