using EventBus.Messages.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Contractor.API.EventBusConsumer
{
    public class ContractorArrangeConsumer : IConsumer<ContractorEvent>
    {
        private readonly ILogger<ContractorArrangeConsumer> _logger;

        public ContractorArrangeConsumer(ILogger<ContractorArrangeConsumer> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Consume(ConsumeContext<ContractorEvent> context)
        {
            _logger.LogInformation("ContractorArrangeConsumer consumed successfully. Created Order Id : {0}", context.Message.Id);
        }
    }
}
