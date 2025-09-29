using API.Data;
using API.DTOs;
using API.Entities;
using API.Entities.Aggregate;
using API.Entities.Enum;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Stripe;

namespace API.Controllers;

[ApiController]
[Route("api/stripe-webhook")]
public class StripeWebhookController : ControllerBase
{
    private readonly RestoreCourseDbContext _context;
    private readonly StripeSettings _stripeSettings;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<StripeWebhookController> _logger;
    
    public StripeWebhookController(RestoreCourseDbContext context, 
        IOptions<StripeSettings> stripeSettings, 
        IHttpContextAccessor httpContextAccessor, 
        ILogger<StripeWebhookController> logger)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
        _stripeSettings = stripeSettings.Value;
    }
    
    [HttpPost]
    public async Task<IActionResult> Post()
    {
           var jsonStripeResult = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

        // var resultStripeSignature = GetKeyValueStripeSignature(_httpContextAccessor.HttpContext.Request.Headers);
        var resultStripeSignatureObject = GetKeyValueStripeSignatureToObject(_httpContextAccessor.HttpContext.Request.Headers);
        
        if (string.IsNullOrEmpty(jsonStripeResult)) return BadRequest("No JSON body found");
        
        // if (string.IsNullOrEmpty(resultStripeSignature)) return BadRequest("Stripe-Signature header is missing");
        
      
        try
        {
            var stripeEvent = EventUtility.ConstructEvent(
                jsonStripeResult,
                resultStripeSignatureObject,
                _stripeSettings.WhSecretKey
            );

            var charge = (Charge)stripeEvent.Data.Object;
            // var stripeConverter = JsonConvert.DeserializeObject<StripeChargerPayment>(jsonStripeResult);
            
            // if (stripeConverter == null) return BadRequest("Could not deserialize JSON body");
            //
            if (!charge.Paid) return BadRequest("No paid");
            //
            // if (stripeConverter.data.DetailPayment.status != "succeeded") return BadRequest("No succeeded");
            

            switch (charge.Status)
            {
                case "succeeded":
                    _logger.LogInformation("PaymentIntent Succeeded");
                    await HandlePaymentSucceededAsync(charge.PaymentIntentId);
                    break;
                case "failed":
                    _logger.LogInformation("PaymentIntent failed");
                    await HandlePaymentFailedAsync(charge.PaymentIntentId);
                    break;                
                default:
                    _logger.LogInformation($"paid: {charge.Paid} - status: {charge.Status} not handled");
                   break;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        return new EmptyResult();
    }

    private string GetKeyValueStripeSignature(IHeaderDictionary requestHeaders)
    {
        if (requestHeaders.TryGetValue("Stripe-Signature", out var stripeSignature))
        {
            return stripeSignature;
        }
        return null;
    }
    
    private StringValues GetKeyValueStripeSignatureToObject(IHeaderDictionary requestHeaders)
    {
        if (requestHeaders.TryGetValue("Stripe-Signature", out StringValues stripeSignature))
        {
            return stripeSignature;
        }
        return StringValues.Empty;
    }

    private async Task HandlePaymentSucceededAsync(string paymentIntentId)
    {
        var order = await _context.DbSet<Order>()
            .FirstOrDefaultAsync(o => o.PaymentIntentId == paymentIntentId);

        if (order != null)
        {
            if (order.OrderStatus == OrderStatus.Pending)
            {
                order.OrderStatus = OrderStatus.PaymentReceived;
                await _context.SaveChangesAsync();
                _logger.LogInformation($"{order.Id} - OrderStatus PaymentReceived");
            }
            else
            {
                _logger.LogInformation($"{order.Id} - Procedded");

            }
        }
        else
        {
            _logger.LogInformation($"Not found order");
        }
    }
    
    private async Task HandlePaymentFailedAsync(string paymentIntentId)
    {
        var order = await _context.DbSet<Order>()
            .FirstOrDefaultAsync(o => o.PaymentIntentId == paymentIntentId);

        if (order != null && order.OrderStatus == OrderStatus.Pending)
        {
            order.OrderStatus = OrderStatus.PaymentFailed;
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Pedido {order.Id} atualizado para Falhou.");

            // Aqui você pode:
            // - Enviar um e-mail ao cliente informando sobre a falha
        }
    }
}