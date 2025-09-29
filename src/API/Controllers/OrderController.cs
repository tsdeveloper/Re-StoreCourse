using API.Data;
using API.DTOs.Orders;
using API.Entities.Aggregate;
using API.Entities.Baskets;
using API.Entities.Enum;
using API.Entities.Products;
using API.Entities.Users;
using API.Extensions.Baskets;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class OrderController : BaseApiController
{
    private readonly RestoreCourseDbContext _context;

    public OrderController(RestoreCourseDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<OrderDto>> GetOrders()
    {
        var orders = await _context.DbSet<Order>()
            .Include(o => o.OrderItems)
            .Where(x => x.BuyerId == User.Identity.Name)
            .OrderBy(s =>
                s.OrderStatus == OrderStatus.PaymentReceived ? 0 :
                s.OrderStatus == OrderStatus.Pending ? 1 : 2)
            .ThenByDescending(x => x.OrderDate)
            .ToListAsync();

        var orderDtoList = orders.Select(x => (OrderDto)x).ToList();
        return Ok(orderDtoList);
    }

    [HttpGet("{id}", Name = "GetOrder")]
    public async Task<ActionResult<OrderDto>> GetOrder(int id)
    {
        var order = await _context.DbSet<Order>()
            .Include(o => o.OrderItems)
            .Where(x => x.BuyerId == User.Identity.Name && x.Id == id)
            .FirstOrDefaultAsync();

        var orderDto = (OrderDto)order;
        return Ok(orderDto);
    }

    [HttpPost]
    public async Task<ActionResult<OrderDto>> CreateOrder(CreateOrderDto orderDto)
    {
        var basket = await _context.DbSet<Basket>()
            .RetrieveBasketWithItems(User.Identity.Name)
            .FirstOrDefaultAsync();

        if (basket == null) return BadRequest(new ProblemDetails { Title = "Could not locate basket" });

        var items = new List<OrderItem>();

        foreach (var item in basket.BasketItems)
        {
            var productItem = await _context.DbSet<Product>()
                .FindAsync(item.ProductId);

            var itemOrdered = new ProductItemOrdered
            {
                ProductId = productItem.Id,
                Name = productItem.Name,
                PictureUrl = productItem.PictureUrl
            };

            var orderItem = new OrderItem
            {
                ItemOrdered = itemOrdered,
                Price = productItem.Price,
                Quantity = item.Quantity
            };

            items.Add(orderItem);
            productItem.QuantityInStock -= item.Quantity;
        }

        var subTotal = items.Sum(x => x.Price * x.Quantity);
        var devileryFee = subTotal > 1000 ? 0 : 500;

        var order = new Order
        {
            BuyerId = User.Identity.Name,
            OrderItems = items,
            ShippingAddress = (ShippingAddress)orderDto.ShippingAddress,
            SubTotal = subTotal,
            DeliveryFee = devileryFee,
            PaymentIntentId = basket.PaymentIntentId
        };

        _context.DbSet<Order>().Add(order);
        _context.DbSet<Basket>().Remove(basket);

        if (orderDto.SaveAddress)
        {
            var user = await _context.DbSet<UserCustom>()
                .Include(a => a.Address)
                .FirstOrDefaultAsync(x => x.UserName == User.Identity.Name);

            var address = new UserAddress
            {
                FullName = order.ShippingAddress.FullName,
                Address1 = order.ShippingAddress.Address1,
                Address2 = order.ShippingAddress.Address2,
                City = order.ShippingAddress.City,
                State = order.ShippingAddress.State,
                Zip = order.ShippingAddress.Zip,
                Country = order.ShippingAddress.Country
            };

            user.Address = address;
        }

        var result = await _context.SaveChangesAsync() > 0;

        if (!result) return BadRequest("Problem creating order");

        return CreatedAtRoute("GetOrder", new { id = order.Id }, order.Id);
    }
}