using API.Entities;
using API.Entities.Baskets;
using Microsoft.Extensions.Options;
using Stripe;

namespace API.Service;

public class PaymentService
{
    private StripeSettings _stripeSettings;

    public PaymentService(IOptions<StripeSettings> stripeSettings)
    {
        _stripeSettings = stripeSettings.Value;
    }

    public async Task<PaymentIntent> CreateOrUpdatePaymentIntent(Basket basket)
    {
        StripeConfiguration.ApiKey = _stripeSettings.SecretKey;
        
        var service  = new PaymentIntentService();

        var intent = new PaymentIntent();
        var subtotal = basket.BasketItems.Sum(x => x.Quantity * x.Product.Price);
        var deliveryFee = subtotal > 1000 ? 0 : 500;

        if (string.IsNullOrEmpty(basket.PaymentIntendId))
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount = (int)(subtotal + deliveryFee),
                Currency = "usd",
                PaymentMethodTypes = new List<string> { "card" },
            };
            intent = await service.CreateAsync(options);
            basket.PaymentIntendId = intent.Id;
        }
        else
        {
            var options = new PaymentIntentUpdateOptions
            {
                Amount = (int)(subtotal + deliveryFee)
            };
            
            await service.UpdateAsync(basket.PaymentIntendId, options);

        }

        return intent;
    }
}