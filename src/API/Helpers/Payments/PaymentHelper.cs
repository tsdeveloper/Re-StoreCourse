using API.DTOs.Payments;
using API.Entities.Enum;
using Stripe;

namespace API.Helpers.Payments;

public static class PaymentHelper
{
    public static PaymentStatus ConvertStatusStripeToPaymentStatus(this PaymentConfirmationResultDTO intent) => intent.Status.ToLower() switch
    {
        "succeeded" => PaymentStatus.Succeeded,
        "processing" => PaymentStatus.Processing,
        _ => PaymentStatus.Failed
    };

}