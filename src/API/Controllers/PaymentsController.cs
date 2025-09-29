using API.Data;
using API.DTOs.Baskets;
using API.DTOs.Payments;
using API.Entities.Aggregate;
using API.Entities.Baskets;
using API.Entities.Enum;
using API.Extensions.Baskets;
using API.Helpers.Orders;
using API.Helpers.Payments;
using API.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class PaymentsController : BaseApiController
{
    private readonly RestoreCourseDbContext _context;
    private readonly PaymentService _paymentService;

    public PaymentsController(PaymentService paymentService, RestoreCourseDbContext context)
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

        if (basket == null) return BadRequest(new ProblemDetails { Title = "Could not locate basket" });

        var intent = await _paymentService.CreateOrUpdatePaymentIntent(basket);

        if (intent == null) return BadRequest(new ProblemDetails { Title = "Could not create payment intent" });

        basket.PaymentIntentId = basket.PaymentIntentId ?? intent.Id;
        basket.ClientSecret = basket.ClientSecret ?? intent.ClientSecret;

        _context.Update(basket);

        var result = await _context.SaveChangesAsync() > 0;

        if (!result) return BadRequest(new ProblemDetails { Title = "Could not save payment intent" });

        return (BasketDTO)basket;
    }

    [Authorize]
    [HttpPost("confirm")]
    public async Task<ActionResult<PaymentConfirmationResultDTO>> ConfirmPaymentIntent([FromBody] BasketDTO basketDto)
    {
        var basket = await _context.DbSet<Basket>()
            .RetrieveBasketWithItems(User.Identity.Name)
            .FirstOrDefaultAsync(b => b.PaymentIntentId == basketDto.PaymentIntentId);;

        if (basket == null) return BadRequest(new ProblemDetails { Title = "Could not locate basket" });

        var intent = await _paymentService.ConfirmPaymentAsync(basket);

        var paymentStatus = intent.ConvertStatusStripeToPaymentStatus();
        
        if (intent == null) return BadRequest(new ProblemDetails { Title = "Could not create payment intent" });

        if (!new []{ PaymentStatus.Succeeded, PaymentStatus.Processing}.Contains(paymentStatus)) return BadRequest(new ProblemDetails { Title = "Failed confirm payment intent" });
        
        var result = await _context.SaveChangesAsync() > 0;

        if (!result) return BadRequest(new ProblemDetails { Title = "Could not confirm payment intent" });

        return intent;
    }
}