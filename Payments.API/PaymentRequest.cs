namespace Payments.API
{
    public class PaymentRequest
    {
        public string CreditCardNumber { get; set; }
        public string ExpirationDate { get; set; }
        public decimal Amount { get; set; }
    }
}