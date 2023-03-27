namespace Payments.API
{
    public class Payment
    {
        public object ClientId { get; set; }
        public decimal Amount { get; set; }
        public object TransactionId { get; set; }
        public object Status { get; set; }
        public DateTime Date { get; set; }
    }
}