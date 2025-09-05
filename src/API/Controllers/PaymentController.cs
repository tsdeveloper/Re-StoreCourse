using API.Data;
using API.DTOs.Baskets;
using API.Entities.Baskets;
using API.Extensions.Baskets;
using API.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class PaymentController : BaseApiController
{
    private readonly PaymentService _paymentService;
    private readonly RestoreCourseDbContext _context;
    public PaymentController(PaymentService paymentService, RestoreCourseDbContext context)
    {
        _paymentService = paymentService;
        _context = context;
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<BasketDTO>> CreateOrUpdatePaymentIntent()
    {
        var basket = await _context.DbSet<Basket>()
            .RetrieveBasketWithItems(User.Identity.Name)
            .FirstOrDefaultAsync();
        
        if (basket == null) return BadRequest(new ProblemDetails { Title = "Could not locate basket"});
        
        var intent = await _paymentService.CreateOrUpdatePaymentIntent(basket);
        
        if (intent == null) return BadRequest(new ProblemDetails { Title = "Could not create payment intent"});
        
        basket.PaymentIntendId = basket.PaymentIntendId ?? intent.Id;
        
        _context.Update(basket);

        var result = await _context.SaveChangesAsync() > 0;
        
        if (!result) return BadRequest(new ProblemDetails { Title = "Could not save payment intent"});
        
        return (BasketDTO)basket;
    }
}