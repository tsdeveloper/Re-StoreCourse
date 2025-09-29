using API.DTOs.Payments;
using API.Entities;
using API.Entities.Aggregate;
using API.Entities.Baskets;
using Microsoft.Extensions.Options;
using Stripe;

namespace API.Service;

public class PaymentService
{
    private readonly StripeSettings _stripeSettings;
    private readonly PaymentIntentService _paymentIntentService;
    private readonly ILogger<PaymentService> _logger;

    public PaymentService(IOptions<StripeSettings> stripeSettings, PaymentIntentService paymentIntentService, ILogger<PaymentService> logger)
    {
        _paymentIntentService = paymentIntentService;
        _logger = logger;
        _stripeSettings = stripeSettings.Value;
    }

    public async Task<PaymentIntent> CreateOrUpdatePaymentIntent(Basket basket)
    {
        StripeConfiguration.ApiKey = _stripeSettings.SecretKey;

        var service = new PaymentIntentService();

        var intent = new PaymentIntent();
        var subtotal = basket.BasketItems.Sum(x => x.Quantity * x.Product.Price);
        var deliveryFee = subtotal > 1000 ? 0 : 500;
        var amount = (long)(subtotal + deliveryFee) * 100;

        if (string.IsNullOrEmpty(basket.PaymentIntentId))
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount = amount,
                Currency = "usd",
                AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
                {
                    Enabled = true,
                },
            };
            intent = await service.CreateAsync(options);
            basket.PaymentIntentId = intent.Id;
            basket.ClientSecret = intent.ClientSecret;
        }
        else
        {
            var options = new PaymentIntentUpdateOptions
            {
                Amount = amount
            };

            await service.UpdateAsync(basket.PaymentIntentId, options);
        }

        return intent;
    }

    public async Task<PaymentConfirmationResultDTO> ConfirmPaymentAsync(Basket basket)
    {
        StripeConfiguration.ApiKey = _stripeSettings.SecretKey;

        try
        {
            // Opções para a confirmação do PaymentIntent
            var confirmOptions = new PaymentIntentConfirmOptions
            {
                PaymentMethod = "pm_card_visa",
                ReturnUrl = "localhost:5000/payment/success"
            };

            // Envia a requisição de confirmação para a Stripe
            PaymentIntent confirmedIntent = await _paymentIntentService.ConfirmAsync(
                basket.PaymentIntentId,
                confirmOptions
            );

            return new PaymentConfirmationResultDTO
            {
                IsSuccess = true,
                Status = confirmedIntent.Status,
                TransactionId = confirmedIntent.Id // Ou confirmedIntent.LatestChargeId
            };
        }
        catch (StripeException ex)
        {
            // Captura erros específicos da API da Stripe (ex: cartão recusado, dados inválidos)
            _logger.LogError(ex, "Erro ao confirmar pagamento na Stripe: {ErrorMessage}", ex.StripeError.Message);
            return new PaymentConfirmationResultDTO
            {
                IsSuccess = false,
                Status = "failed",
                ErrorMessage = ex.StripeError.Message // Mensagem de erro amigável da Stripe
            };
        }
        catch (Exception ex)
        {
            // Captura erros genéricos da aplicação
            _logger.LogError(ex, "Erro inesperado no serviço de pagamento.");
            return new PaymentConfirmationResultDTO
            {
                IsSuccess = false,
                Status = "error",
                ErrorMessage = "Ocorreu um erro inesperado no servidor."
            };
        }
    }
}