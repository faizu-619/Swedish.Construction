namespace Payments.API
{
    public class NullPaymentAPI : IPaymentAPI
    {
        public async Task<Payment> ProcessPaymentAsync(PaymentRequest paymentRequest)
        {
            return await Task.FromResult(new Payment() { });
        }
    }
}
