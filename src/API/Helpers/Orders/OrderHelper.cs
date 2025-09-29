using API.DTOs.Payments;
using API.Entities.Enum;
using Stripe;

namespace API.Helpers.Orders;

public static class OrderHelper
{
    public static OrderStatus ConvertStatusStripeToOrderStatus(this PaymentConfirmationResultDTO intent) => intent.Status.ToLower() switch
    {
        "succeeded" => OrderStatus.PaymentReceived,
        "processing" => OrderStatus.Pending,
        _ => OrderStatus.PaymentFailed
    };

}