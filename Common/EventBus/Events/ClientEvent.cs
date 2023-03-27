namespace EventBus.Messages.Events
{
    public class ClientEvent : IntegrationBaseEvent
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? CreditCardNumber { get; set; }
    }
}
