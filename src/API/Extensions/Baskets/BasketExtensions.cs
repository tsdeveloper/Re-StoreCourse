using API.Entities.Baskets;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions.Baskets;

public static class BasketExtensions
{
    public static IQueryable<Basket> RetrieveBasketWithItems(this IQueryable<Basket> query, string buyerId)
    {
        return query.Include(x => x.BasketItems)
            .ThenInclude(x => x.Product)
            .Where(x => x.BuyerId == buyerId);
    }
}