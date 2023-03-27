namespace Payments.API
{
    public interface IPaymentAPI
    {
        Task<Payment> ProcessPaymentAsync(PaymentRequest paymentRequest);
    }
}