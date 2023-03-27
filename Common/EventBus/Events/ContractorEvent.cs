namespace EventBus.Messages.Events
{
    public class ContractorEvent : IntegrationBaseEvent
    {
        public long Id { get; set; }
        public string? RequesterName { get; set; }
        public string? RequesterEmail { get; set; }
        public string? RequestDetails { get; set; }
    }
}
