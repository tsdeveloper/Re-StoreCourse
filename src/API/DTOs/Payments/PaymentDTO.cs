namespace API.DTOs.Payments;

public class PaymentConfirmationResultDTO
{
    public bool IsSuccess { get; init; }
    public string Status { get; init; }
    public string? TransactionId { get; init; }
    public string? ErrorMessage { get; init; }
}