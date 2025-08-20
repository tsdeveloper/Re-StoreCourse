using API.Data;
using API.DTOs.Orders;
using API.Entities.Aggregate;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class OrdersController : BaseApiController
{
   private readonly RestoreCourseDbContext _context;

   public OrdersController(RestoreCourseDbContext context)
   {
      _context = context;
   }

   [HttpGet]
   public async Task<ActionResult<OrderDto>> GetOrders()
   {
      var orders =  await _context.DbSet<Order>()
         .Include(o => o.OrderItems)
         .Where(x => x.BuyerId == User.Identity.Name)
         .ToListAsync();

      var orderDtoList = orders.Select(x => (OrderDto)x).ToList();
      return Ok(orderDtoList);
   }
   
   [HttpGet("{id}")]
   public async Task<ActionResult<OrderDto>> GetOrder(int id)
   {
      var order =  await _context.DbSet<Order>()
         .Include(o => o.OrderItems)
         .Where(x => x.BuyerId == User.Identity.Name && x.Id == id)
         .FirstOrDefaultAsync();

      var orderDto = (OrderDto)order;
      return Ok(orderDto);
   }

}