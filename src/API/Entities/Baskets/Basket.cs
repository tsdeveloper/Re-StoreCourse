using API.Entities.Products;

namespace API.Entities.Baskets;

public class Basket : BaseEntity
{
    public string BuyerId { get; set; }
    public List<BasketItem> BasketItems { get; set; } = new();

    public void AddItem(Product product, int quantity)
    {
        if (product == null) ArgumentNullException.ThrowIfNull(product);
        if (quantity <= 0)
            throw new ArgumentNullException("Quantity should be greater than zero",
                nameof(quantity));

        var existingItem = FindItem(product.Id);

        if (existingItem == null)
        {
            BasketItems.Add(new BasketItem
            {
                Product = product,
                Quantity = quantity
            });
        }
        else
        {
            existingItem.Quantity += quantity;
        }
    }

    private BasketItem? FindItem(int productId)
    {
        return BasketItems.FirstOrDefault(item => item.ProductId == productId);
    }

    public void RemoveItem(int productId, int quantity)
    {
        var existingItem = BasketItems.FirstOrDefault(x => x.ProductId == productId);
        if (existingItem == null) return;

        if (existingItem.Quantity > quantity && existingItem.Quantity > 0)
            existingItem.Quantity -= quantity;
        else 
            existingItem.Quantity = 0;

        if (existingItem.Quantity == 0) BasketItems.Remove(existingItem);
    }
}